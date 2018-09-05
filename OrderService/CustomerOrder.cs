namespace OrderService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using EmailingService;

    using FileStorageService;

    using HtmlAgilityPack;

    using LeadService;

    using Mandrill;

    using MeetUp.Common;
    using MeetUp.Enumerations;
    using MeetUp.Model;

    using NLog;

    public class CustomerOrder : ICustomerOrder
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private Guid ServiceId => SystemUsers.OrderService;

        #region Private Data member

        private readonly ISupplierService _supplierService;
        private readonly IOrderFormService _formService;
        private readonly IFileStore _fileStore;
        private readonly IMailMessageService _mailMessageService;
        private readonly ICustomerOrderService _customerOrderService;
        private readonly IMailAttachmentService _mailAttachmentService;
        private readonly ICustomerLeadStatusService _customerLeadStatusService;

        #endregion

        #region Constructors

        public CustomerOrder(IOrderFormService formService, ICustomerOrderService customerOrderService, ISupplierService supplierService,
            IMailMessageService mailMessageService, IFileStore fileStore, IMailAttachmentService mailAttachmentService, ICustomerLeadStatusService customerLeadStatusService)
        {
            _fileStore = fileStore;
            _supplierService = supplierService;
            _mailMessageService = mailMessageService;
            _customerOrderService = customerOrderService;
            _mailAttachmentService = mailAttachmentService;
            _customerLeadStatusService = customerLeadStatusService;
            _formService = formService;
        }

        #endregion

        #region Public Methods

        // Sending Orders of the customer to respective suppliers
        public bool SendCustomerOrders(int dayTotalHours, string subjectLine, int dayTotalMinutes, Boolean isHourBased, string orderFromEmail, string supportEmail)
        {
            //ResendEmail();
            // Getting 
            List<Supplier> supplierList = _supplierService.GetSuppliersForSendingOrder();

            if (supplierList == null || supplierList.Count <= 0)
            {
                return false;
            }

            foreach (Supplier supplier in supplierList)
            {
                if (supplier.SupplierOrderPolicies == null || supplier.SupplierOrderPolicies.Count <= 0)
                {
                    continue;
                }
                Boolean sendOrder = true;

                if (sendOrder != true)
                {
                    continue;
                }

                List<Customer_Order> customerOrderList = _customerOrderService.GetNewOrders(supplier.SupplierId);
                Logger.Info($"Pending Orders For {supplier.Name} = {customerOrderList.Count}");
                foreach (Customer_Order customerOrder in customerOrderList)
                {
                    try
                    {
                        if (!supplier.SupplierOrderPolicies.ToList()[0].SendOrderViaEmail || (string.IsNullOrEmpty(supplier.SupplierOrderPolicies.ToList()[0].OrderEmailAddress)))
                        {
                            continue;
                        }

                        var role = customerOrder.Customer.UserRolesInCustomers.FirstOrDefault(i => i.AspNetUserId == customerOrder.CreatedByUserId) ??
                                   customerOrder.Customer.UserRolesInCustomers.FirstOrDefault(i => i.AspNetRole.Name == "Admin");

                        var user = role?.AspNetUser;
                        if (user == null)
                        {
                            continue;
                        }

                        string[] orderSubjctline = subjectLine.Split(',');
                        var emailSubject = user.LanguageCode == LanguageCode.English ? orderSubjctline[0] : orderSubjctline[1];

                        Boolean supplierEmployeeLimitForOrder = true;
                        if (customerOrder.Customer.NoOfEmployee < supplier.SupplierOrderPolicies.ToList()[0].MinEmployeeLimitForOrder)
                        {
                            supplierEmployeeLimitForOrder = false;

                            if (string.IsNullOrEmpty(supplier.InternalSupportEmailForSpecialSituations))
                            {
                                continue;
                            }
                        }

                        var response = SaveLeadStatus(customerOrder.ForSupplierId, customerOrder.FromCustomerId, customerOrder.FromDepartmentId, user.Id, supplierEmployeeLimitForOrder, customerOrder.Id);
                        if (response != null && response.IsSuccess)
                        {
                            var emailHtmlBody = "";
                            if (customerOrder.IsJson == null || customerOrder.IsJson == false)
                            {
                                Logger.Info($"Orders# {customerOrder.Id} = is XML");
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(customerOrder.OrderXML);

                                //XmlNode XmlOrder = xmlDoc.SelectSingleNode("//Order");
                                XmlNode xmlOrderDetail = xmlDoc.SelectSingleNode("//OrderDetail");


                                XmlElement fieldNode = xmlDoc.CreateElement("Field");
                                xmlOrderDetail.AppendChild(fieldNode);

                                //Creating Child Node of the Key
                                XmlElement childKey = xmlDoc.CreateElement("Key");
                                //childKey.Attributes.Append(xmlDoc.CreateAttribute("name", "trackTid"));
                                childKey.InnerText = "TrackingLeadId";

                                fieldNode.AppendChild(childKey);

                                //Creating Child Node of the Value
                                XmlElement childValue = xmlDoc.CreateElement("Value");
                                childValue.InnerText = response.TrackingLeadId.ToString();

                                fieldNode.AppendChild(childValue);

                                customerOrder.OrderXML = xmlDoc.OuterXml;
                                emailHtmlBody = new XSLTransformHelper().CreateHTML(customerOrder.OrderXML, user.LanguageCode);
                            }
                            else
                            {
                                Logger.Info($"Orders# {customerOrder.Id} = is XML");
                                emailHtmlBody = _formService.ConvertToHTML(customerOrder.OrderXML, response.TrackingLeadId, customerOrder.Id).Message;
                            }

                            Logger.Info("Orders# {0} HTML = " + emailHtmlBody, customerOrder.Id);

                            if (!string.IsNullOrEmpty(emailHtmlBody))
                            {
                                HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                doc.LoadHtml(emailHtmlBody);

                                var tbl = doc.GetElementbyId("tbl");
                                var nodes = tbl.SelectNodes("tr/td");
                                foreach (var node in nodes)
                                {
                                    if (node.InnerText.Contains(response.TrackingLeadId.ToString()))
                                    {
                                        node.Attributes.Append("name", "trackTid");
                                    }
                                }

                                emailHtmlBody = doc.DocumentNode.InnerHtml;
                            }

                            // department email, and support email.
                            orderFromEmail = customerOrder.OrderEmail.Equals(supportEmail)
                                ? orderFromEmail
                                : customerOrder.Department.InternalEmail;
                            var attachments = GetEmailAttachment(customerOrder);
                            Logger.Info(String.Format("Orders# {0} has attachments = " + attachments?.Count,
                                customerOrder.Id));
                            if (supplierEmployeeLimitForOrder)
                            {
                                string recipientEmail = (string.IsNullOrEmpty(customerOrder.OrderEmail)
                                    ? supplier.SupplierOrderPolicies.ToList()[0].OrderEmailAddress
                                    : customerOrder.OrderEmail);
                                ////get email attachments

                                MailMessageResponse message = SaveMailMessage(user.Id, supplier.Name, emailSubject,
                                    emailHtmlBody, user.InternalEmail, recipientEmail, response.TrackingLeadId,
                                    customerOrder.FromCustomerId, customerOrder.FromDepartmentId);
                                if (customerOrder.CustomerOrderAttachments.Count > 0)
                                {
                                    SaveCustomerAttachmentsInMailMessage(customerOrder.CustomerOrderAttachments,
                                        message.MessageId);
                                }

                                SendEmail(user, emailHtmlBody, recipientEmail,
                                    customerOrder.Customer.ChannelId.ToString(), orderFromEmail, user.LanguageCode,
                                    supplier.Name, customerOrder.Customer.CustomerName, attachments);

                                var html = _formService.GetOrderQuestionsHtml(customerOrder.Id);

                                SendOrderToPrimaryEmail(user, emailSubject, html, response.TrackingLeadId,
                                    supplier.Name, attachments, customerOrder.Customer.ChannelId.ToString(),
                                    customerOrder.FromCustomerId, customerOrder.FromDepartmentId,
                                    customerOrder.CustomerOrderAttachments);
                            }
                            else
                            {
                                MailMessageResponse message = SaveMailMessage(user.Id, supplier.Name, emailSubject,
                                    emailHtmlBody, user.InternalEmail,
                                    supplier.InternalSupportEmailForSpecialSituations, response.TrackingLeadId,
                                    customerOrder.FromCustomerId, customerOrder.FromDepartmentId);
                                if (customerOrder.CustomerOrderAttachments.Count > 0)
                                {
                                    SaveCustomerAttachmentsInMailMessage(customerOrder.CustomerOrderAttachments,
                                        message.MessageId);
                                }

                                // Sending Email to Supplier
                                SendEmail(user, emailHtmlBody, supplier.InternalSupportEmailForSpecialSituations,
                                    customerOrder.Customer.ChannelId.ToString(), orderFromEmail, user.LanguageCode,
                                    supplier.Name, customerOrder.Customer.CustomerName, attachments);

                                var html = _formService.GetOrderQuestionsHtml(customerOrder.Id);

                                SendOrderToPrimaryEmail(user, emailSubject, html, response.TrackingLeadId,
                                    supplier.Name, attachments, customerOrder.Customer.ChannelId.ToString(),
                                    customerOrder.FromCustomerId, customerOrder.FromDepartmentId,
                                    customerOrder.CustomerOrderAttachments);
                            }

                            customerOrder.Status = OrderStatus.Sent;
                            customerOrder.SentDateTimeUtc = DateTime.UtcNow;

                            _customerOrderService.SaveCustomerOrder(customerOrder);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Fatal(ex, $"Orders #{customerOrder.Id} has exception");
                    }
                }
            }

            return true;
        }


        #endregion

        #region private method

        private CustomerLeadServiceResponse SaveLeadStatus(int supplierId, int customerId, int departmentId, Guid userId, bool supplierEmployeeLimitForOrder, Guid orderId)
        {
            CustomerLeadStatus leadStatus = new CustomerLeadStatus
            {
                ForSupplierId = supplierId,
                FromCustomerId = customerId,
                FromDepartmentId = departmentId,
                FromUserId = userId,
                OrderId = orderId,
                CreatedBy = this.ServiceId, // not directly by user, disputable 
                CreatedDateTimeUtc = DateTime.UtcNow
            };

            CustomerLeadStatusDetail leadDetail = new CustomerLeadStatusDetail
            {
                LeadStatus = (supplierEmployeeLimitForOrder) ? LeadStatus.PendingSupplier : LeadStatus.PendingInternal,
                UserComments = (supplierEmployeeLimitForOrder) ? @"Waiting for Supplier response." : @"Waiting for Support response. Lead has been sent to Support.",
                CreatedBy = this.ServiceId, // not directly by user, disputable 
                CreatedDateTimeUtc = DateTime.UtcNow
            };

            leadStatus.CustomerLeadStatusDetails.Add(leadDetail);

            return _customerLeadStatusService.SaveCustomerLead(leadStatus);
        }

        private Boolean SendEmail(AspNetUser user, string message, string DestinationEmail, string channelId, string SenderEmail, LanguageCode culture, string RecipientName, string SenderName, List<email_attachment> attachmentList)
        {
            EmailProvider client = new EmailProvider(channelId);

            Dictionary<string, string> emailContents = new Dictionary<string, string>();
            emailContents.Add("TABLEBODY", message);
            emailContents.Add("from", SenderEmail);
            emailContents.Add("NAME", RecipientName);
            emailContents.Add("MEMBER", SenderName);
            //emailContents.Add("cc", SenderEmail);
            bool sent = false;
            if (attachmentList != null && attachmentList.ToList().Count > 0)
            {
                sent = client.Send(DestinationEmail, emailContents, client.GetTemplateType("CustomerOrder", culture), attachmentList);
            }
            else
            {
                sent = client.Send(DestinationEmail, emailContents, client.GetTemplateType("CustomerOrder", culture));
            }

            Logger.Info(String.Format("Sent {0}", sent));
            Logger.Info(String.Format("Email Sent to {0} with Email {1}", RecipientName, DestinationEmail));
            return true;
        }

        public Boolean ResendEmail(List<int> leadIds)
        {
            string targetemail = "mij@yopmail.com";
            foreach (var leadid in leadIds)
            {
                List<MailMessage> messages = _mailMessageService.GetParentLeadMessage(leadid);
                MailMessage mesg = messages.FirstOrDefault(p => p.DisplayName.Equals("Circle K"));

                SendEmail(messages.FirstOrDefault().AspNetUser, messages.FirstOrDefault().Body, targetemail, messages.FirstOrDefault().Customer.ChannelId.ToString(),
                    messages.FirstOrDefault().FromAddress, messages.FirstOrDefault().AspNetUser.LanguageCode, messages.FirstOrDefault().DisplayName, messages.FirstOrDefault().Customer.CustomerName, null);
            }

            return true;
        }

        private void SendOrderToPrimaryEmail(AspNetUser user, string subject, string body, int leadId, string supplier, List<email_attachment> attachmentList, string channelId, int customerId, int deptId, ICollection<CustomerOrderAttachment> customer_Order_Attachment)
        {
            try
            {
                Logger.Info(String.Format("Email Sending to {0} with PrimaryEmail {1}", user.FirstName, user.Email));
                EmailProvider client = new EmailProvider(channelId);

                var cwaUrl = ConfigurationManager.AppSettings["ChannelOneLoginUrl"] + channelId;

                if (channelId == "12")
                {
                    cwaUrl = ConfigurationManager.AppSettings["ChannelTwoLoginUrl"] + channelId;
                }

                var emailContents = new Dictionary<string, string>();
                emailContents.Add("SUPPLIER", supplier);
                emailContents.Add("NAME", user.FirstName + " " + user.LastName);
                emailContents.Add("URL", cwaUrl);
                emailContents.Add("TABLEBODY", body);

                var type = client.GetTemplateType("OfferRequestOrder", user.LanguageCode, Convert.ToInt32(channelId));
                var slug = client.GetTemplateName(type);

                bool sent = false;
                if (attachmentList != null && attachmentList.ToList().Count > 0)
                {
                    sent = client.Send(user.Email, emailContents, type, attachmentList);
                }
                else
                {
                    sent = client.Send(user.Email, emailContents, type);
                }

                var mail = new MailMessage()
                {
                    CustomerId = customerId,
                    DepartmentId = deptId,
                    LeadTrackingId = leadId,
                    Kind = MessageKind.Received,
                    ToAddress = user.Email,
                    UserId = user.Id,
                    HideFromUser = true,
                    CreatedBy = this.ServiceId,
                    CreatedDateTimeUtc = DateTime.UtcNow
                };

                _mailMessageService.SaveMailMessage(mail, slug, channelId, emailContents);
                if (attachmentList != null && attachmentList.ToList().Count > 0)
                {
                    SaveCustomerAttachmentsInMailMessage(customer_Order_Attachment, mail.MessageId);
                }

                Logger.Info($"Email Sent to {user.FirstName} with PrimaryEmail {user.Email}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private MailMessageResponse SaveMailMessage(Guid userId, string name, string subject, string message, string senderEmail, string destinationEmail, int leadTrackingId, int companyId, int deptId)
        {
            MailMessage mailMessage = new MailMessage
            {
                UserId = userId,
                DisplayName = name,
                FromAddress = senderEmail,
                ToAddress = destinationEmail,
                Subject = subject,
                Body = message,
                LeadTrackingId = leadTrackingId,
                Status = MessageStatus.Read,
                CustomerId = companyId,
                DepartmentId = deptId,
                Kind = MessageKind.Sent,
                HideFromUser = true,
                Type = MessageType.Order,
                CreatedBy = this.ServiceId,
                CreatedDateTimeUtc = DateTime.UtcNow
            };

            return _mailMessageService.SaveMailMessage(mailMessage);
        }

        private List<email_attachment> GetEmailAttachment(Customer_Order customerOrder)
        {
            if (customerOrder.CustomerOrderAttachments != null && customerOrder.CustomerOrderAttachments.ToList().Count > 0)
            {
                List<email_attachment> emailAttachmentList = new List<email_attachment>();

                foreach (var item in customerOrder.CustomerOrderAttachments.ToList())
                {
                    var destination = ConfigurationManager.AppSettings["TempDir"].ToString();
                    if (!Directory.Exists(destination))
                    {
                        Directory.CreateDirectory(destination);
                    }

                    string fileUrl = _fileStore.Download(item.FileURL, destination);

                    if (!string.IsNullOrEmpty(fileUrl) && File.Exists(fileUrl))
                    {
                        byte[] filedata = File.ReadAllBytes(fileUrl);
                        var base64 = Convert.ToBase64String(filedata);

                        emailAttachmentList.Add(new email_attachment
                        {
                            content = base64,
                            name = item.FileName,
                            type = item.MimeType
                        });

                        File.Delete(fileUrl);
                    }
                }

                return emailAttachmentList;
            }

            return null;
        }

        private bool SaveCustomerAttachmentsInMailMessage(ICollection<CustomerOrderAttachment> customerOrderAttachment, Guid messgeId)
        {

            foreach (var item in customerOrderAttachment)
            {
                MailAttachment attachment = new MailAttachment
                {
                    MessageId = messgeId,
                    FileName = item.FileName,
                    FileUrl = item.FileURL,
                    FilePath = item.FilePath,
                    MimeType = item.MimeType,
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    CreatedBy = this.ServiceId
                };

                _mailAttachmentService.SaveAttachemnts(attachment);
            }
            return true;
        }

        

        #endregion

    }
}