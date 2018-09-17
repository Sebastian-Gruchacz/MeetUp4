namespace OrderFormService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::OrderFormService.ExternalModel;
    using global::OrderFormService.OtherServices;

    using MeetUp.Common;
    using MeetUp.Enumerations;
    using MeetUp.Model;

    using Newtonsoft.Json;

    using NLog;

    public class OrderFormService : IOrderFormService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ITranslationsRepository _translationRepository;
        private readonly ISupplierService _supplierService;
        private readonly ICustomerService _customerService;
        private readonly IUserAccountService _accountService;
        private readonly ICustomerOrderService _customerOrderService;
        private readonly IOrderFormHistoryRepository _typeFormOrdersLogRepository;
        private readonly ISupplierCaseLimitsService _supplierCaseLimitsService;
        private readonly ISupplierCustomerNumberRepository _supplierCustomerNumber;

        public OrderFormService(ITranslationsRepository translationRepository, ISupplierService supplierService,
               ICustomerService customerService, IUserAccountService accountService, ICustomerOrderService customerOrderService,
            IOrderFormHistoryRepository typeFormOrdersLogRepository, ISupplierCaseLimitsService supplierCaseLimitsService,
            ISupplierCustomerNumberRepository suplierCustomerNumber)
        {
            _translationRepository = translationRepository ?? throw new ArgumentNullException(nameof(translationRepository));
            _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _customerOrderService = customerOrderService ?? throw new ArgumentNullException(nameof(customerOrderService));
            _typeFormOrdersLogRepository = typeFormOrdersLogRepository ?? throw new ArgumentNullException(nameof(typeFormOrdersLogRepository));
            _supplierCaseLimitsService = supplierCaseLimitsService ?? throw new ArgumentNullException(nameof(supplierCaseLimitsService));
            _supplierCustomerNumber = suplierCustomerNumber ?? throw new ArgumentNullException(nameof(suplierCustomerNumber));
        }

        /// <inheritdoc />
        public ServiceResponse ConvertToHTML(string json, int caseId, Guid orderId)
        {
            ServiceResponse response = new ServiceResponse();
            var order = JsonConvert.DeserializeObject<ExternalOrderFormServiceResponseDto>(json);
            if (!ValidateRequest(order))
            {
                response.IsSuccess = false;
                response.Message = "Invalid Param";
                return response;
            }
            int supplierid = Convert.ToInt32(order.responses.FirstOrDefault().hidden.supplierid);
            Guid userid = Guid.Parse(order.responses.FirstOrDefault().hidden.userid);
            int departmentid = Convert.ToInt32(order.responses.FirstOrDefault().hidden.departmentid);
            int customerid = Convert.ToInt32(order.responses.FirstOrDefault().hidden.customerid);
            int channel = Convert.ToInt32(order.responses.FirstOrDefault().hidden.channel);

            Supplier supplier = _supplierService.Find(supplierid);
            Customer member = _customerService.Find(customerid);
            Department department = member.Departments.FirstOrDefault(d => d.Id == departmentid);
            AspNetUser user = _accountService.GetUser(userid);
            Customer_Order customerOrder = _customerOrderService.GetNewOrders(orderId);

            string html = "<html><body>{0}</body></html>";
            string body = GetHtmlBody(user, supplier, member, department);
            string rows = ConvertLineToHtml(customerOrder.OrderLines.ToList(), caseId.ToString(), user.LanguageCode);

            body = String.Format(body, rows);
            html = String.Format(html, body);

            response.IsSuccess = true;
            response.Message = html;
            return response;
        }

        private string ConvertLineToHtml(List<OrderLine> lines, string caseId, LanguageCode code)
        {
            string rows = "";
            Dictionary<string, string> fields = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(caseId))
                rows += "<tr><td valign='middle'>" + _translationRepository.GetTranslation("TrackingCase", code) + "</td><td valign='middle' style='font-weight:normal;'>" + caseId + "</td></tr>";

            foreach (var line in lines)
            {
                rows += "<tr><td valign='middle'>" + line.LineKey.Replace("<strong>", " ").Replace("</strong>", " ") + "</td><td>" + line.LineValue + "</td></tr>"; // TODO: translations required
            }

            return rows;
        }

        private string ConvertLineToHtml(List<OrderLine> lines, LanguageCode lang)
        {
            string rows = "<table class='table-bordered' id ='tbl' cellpadding='7' cellspacing='0' border = '1' >";
            rows += "<tr>";
            rows += "<th>" + _translationRepository.GetTranslation("CaseHeading-Quest", lang) + "</th>";
            rows += "<th class='text-right'>" + _translationRepository.GetTranslation("CaseHeading-Ans", lang) + "</th>";
            rows += "</tr>";
            foreach (var line in lines)
            {
                rows += "<tr><td valign='middle'>" + line.LineKey.Replace("<strong>", " ").Replace("</strong>", " ") + "</td><td>" + line.LineValue + "</td></tr>"; // TODO: translations required
            }
            rows += "</table>";
            return rows;
        }

        /// <inheritdoc />
        public string GetOrderQuestionsHtml(Guid customerOrderId)
        {
            string html = string.Empty;

            var log = _typeFormOrdersLogRepository.All.FirstOrDefault(o => o.OrderId == customerOrderId);
            if (log != null)
            {
                int cardCount = 0;
                var order = _customerOrderService.GetNewOrders(customerOrderId);
                AspNetUser user = _accountService.GetUser(order.CreatedByUserId);
                var orderDto = JsonConvert.DeserializeObject<ExternalOrderFormServiceResponseDto>(log.Json);
                var orderLines = GetOrderRowsLine(orderDto, order.ForSupplierId, order.FromCustomerId, out cardCount, user)[0];
                html = ConvertLineToHtml(orderLines, user.LanguageCode);
            }


            return html;
        }

        private List<List<OrderLine>> GetOrderRowsLine(ExternalOrderFormServiceResponseDto order, int supplierId, int companyId, out int cardCount, AspNetUser user)
        {
            Logger.Debug("Generating order rows");

            int usage = 0, userOrderCount = 1;
            int count = AllowedItemsInOrder(supplierId, out usage, companyId);
            int numberOfCards = 0;

            List<OrderLine> rows = new List<OrderLine>();
            List<OrderLine> rowsForSupport = new List<OrderLine>();
            List<List<OrderLine>> response = new List<List<OrderLine>>();
            string values = "";
            List<Question> all = null;
            Dictionary<string, string> fields = new Dictionary<string, string>();

            foreach (var line in order.questions)
            {
                try
                {
                    values = "";

                    if (fields.ContainsKey(line.field_id))
                    {
                        continue;
                    }

                    all = order.questions.Where(q => q.field_id == line.field_id && !q.id.Contains("fileupload")).ToList();
                    foreach (var key in all)
                    {
                        if (!order.responses.FirstOrDefault().answers.ContainsKey(key.id))
                        {
                            continue;
                        }

                        if (key.id.Contains("date"))   // Unix to Date Conversion
                        {
                            var date = order.responses.FirstOrDefault().answers[key.id];
                            Logger.Debug("Converting date: {0}", date);

                            try
                            {
                                values += DateTime.UtcNow.ToDateTime(Convert.ToInt64(date.Substring(0, 10))).ToString("dd-MM-yyyy") + " ,";
                            }
                            catch (Exception ex)
                            {
                                Logger.Debug(ex, "Error converting date");
                                values += DateTime.Parse(date).ToString("dd-MM-yyyy") + ",";
                            }
                        }
                        else if (count > 0)
                        {
                            // for limit suppliers
                            var yesNoValue = order.responses.FirstOrDefault().answers.FirstOrDefault(d => d.Key == key.id).Value;
                            if (key.id.Contains("yesno") && yesNoValue == "1")
                                userOrderCount++;
                            if (!key.id.Contains("yesno")) //dont add add more field
                                values += order.responses.FirstOrDefault().answers[key.id] + " ,";
                        }
                        else
                        { //normal fields
                            if (key.id.Contains("yesno"))
                            {
                                if (order.responses.FirstOrDefault().answers[key.id].Equals("1"))
                                    values += _translationRepository.GetTranslation("CaseEmail-Yes", user.LanguageCode) + " ,";
                                else
                                    values += _translationRepository.GetTranslation("CaseEmail-No", user.LanguageCode) + " ,";
                            }
                            else
                                values += order.responses.FirstOrDefault().answers[key.id] + " ,";
                        }
                    }

                    if (!string.IsNullOrEmpty(values))
                    {
                        var result = values.Substring(0, values.Length - 1);
                        if (count > 0) //to add support or normal order
                        {
                            if (usage + userOrderCount <= count)
                            {
                                numberOfCards = userOrderCount;

                                rows.Add(new OrderLine() { LineKey = line.question, LineValue = result });
                            }
                            else
                            {
                                rowsForSupport.Add(new OrderLine() { LineKey = line.question, LineValue = result });
                            }
                        }
                        else
                        {
                            if (line.question.Contains("{{answer_"))
                                line.question = line.question.Substring(0, line.question.IndexOf(":", StringComparison.Ordinal) + 1) + "</strong>";
                            if (line.question.Contains("{{hidden_suppliername}}"))
                                line.question = line.question.Replace("{{hidden_suppliername}}", "");
                            if (!string.IsNullOrWhiteSpace(result))
                                rows.Add(new OrderLine() { LineKey = line.question, LineValue = result });
                        }
                    }
                    fields.Add(line.field_id, "added");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error generating order line");
                }
            }
            cardCount = numberOfCards;
            response.Add(rows);
            response.Add(rowsForSupport);
            return response;
        }

        public int AllowedItemsInOrder(int supplierId, out int sentItemInOrder, int companyId)
        {
            int noOfItems = 0;
            int sentItems = 0;

            var allLimitData = _supplierCaseLimitsService.GetAllSupplierLimitations();
            if (allLimitData.Count > 0)
            {
                if (allLimitData.Any(d => d.SupplierId == supplierId))
                {
                    var customer = _customerService.Find(companyId);
                    customer.PaymentMethod = customer.PaymentMethod != null ? customer.PaymentMethod.ToLower() : null;
                    customer.InternalCreditRating = customer.InternalCreditRating != null ? customer.InternalCreditRating.ToLower() : null;

                    if (customer != null)
                    {
                        customer.PaymentMethod = customer.PaymentMethod != null ? customer.PaymentMethod.ToUpper() : null;
                        customer.InternalCreditRating = customer.InternalCreditRating != null ? customer.InternalCreditRating.ToUpper() : null;
                        foreach (var limit in allLimitData.Where(d => d.SupplierId == supplierId))
                        {
                            limit.CreditRating = limit.CreditRating != null ? limit.CreditRating.ToUpper() : null;
                            limit.PaymentMethod = limit.PaymentMethod != null ? limit.PaymentMethod.ToUpper() : null;
                            if (limit.CreditRating == customer.InternalCreditRating && limit.PaymentMethod == customer.PaymentMethod)
                            {
                                noOfItems = limit.MaxCaseLimit ?? 0;
                                //  break;
                            }
                            else if (limit.CreditRating == customer.InternalCreditRating)
                            {
                                noOfItems = limit.MaxCaseLimit ?? 0;
                                // break;
                            }
                        }

                    }
                    List<int> supplierIds = allLimitData.Select(d => d.SupplierId).Distinct().ToList();
                    int supplierCustomerNumberCount = _supplierCustomerNumber.MemberSupplierCustomerNumbersCountBySupplierIds(supplierIds, companyId);
                    sentItems = supplierCustomerNumberCount + _customerOrderService.GetOrderItemSumForCustomer(supplierIds, companyId);
                }
            }
            sentItemInOrder = sentItems;
            return noOfItems;
        }


        public bool ValidateRequest(ExternalOrderFormServiceResponseDto order)
        {
            if (order == null)
            {
                return false;
            }

            if (order.responses == null)
            {
                return false;

            }
            if (order.responses.FirstOrDefault().hidden == null)
            {
                return false;
            }
            if (order.responses.FirstOrDefault().hidden.supplierid == null)
            {
                return false;
            }
            if (order.responses.FirstOrDefault().hidden.customerid == null)
            {
                return false;
            }
            if (order.responses.FirstOrDefault().hidden.userid == null)
            {
                return false;
            }
            if (order.responses.FirstOrDefault().hidden.departmentid == null)
            {
                return false;
            }
            if (order.responses.FirstOrDefault().hidden.channel == null)
            {
                return false;
            }
            return true;
        }

        private string GetHtmlBody(AspNetUser user, Supplier supplier, Customer member, Department department)
        {
            string body = "<br /><h3 style='line-height:22px;'>" + _translationRepository.GetTranslation("CaseEmail-CompanyInfo", user.LanguageCode) + "</h3>";
            if (supplier.SupplierCaseFields != null && supplier.SupplierCaseFields.Count > 0)
            {
                if (department != null)
                {
                    foreach (var field in supplier.SupplierCaseFields.Where(chanel => chanel.ChannelId == member.ChannelId))
                    {

                        if (field.CaseField.FieldValue == SupplierLoginRequestField.CustomerTitle)
                        {
                            body += "<br /><span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-CompanyTitle", user.LanguageCode) + ": <span style='line-height:23px;'>" + member.CustomerName + "</span></span><br />";
                        }
                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.VatRegistrationNumber)
                        {
                            body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-VatRegNo", user.LanguageCode) + ": <span style='line-height:23px;'>" + member.TaxRegistrationNumber + "</span></span><br />";
                        }

                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.CustomerDepartmentId) // ERR?: Field code does not match extracted fields?
                        {
                            body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-CustomerId", user.LanguageCode) + ": <span style='line-height:23px;'>" + department.ExternalId + "</span></span><br />";
                        }
                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.CustomerInvoiceAddress)
                        {
                            body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-Address", user.LanguageCode) + ": <span style='line-height:23px;'>" + department.Street + " " + department.PostalCode + " " + department.City + ",</span></span><br/>";
                        }
                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.CustomerPhone)
                        {
                            body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-Phone", user.LanguageCode) + ": <span style='line-height:23px;'>" + user.PhoneNumber + "</span></span><br/>";
                        }
                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.CustomerUserName)
                        {
                            body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-Contact", user.LanguageCode) + ": <span style='line-height:23px;'>" + user.FirstName + " " + user.LastName + "</span></span><br/>";
                        }
                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.CustomerUserInternalEmail)
                        {
                            body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-Email", user.LanguageCode) + ": <span style='line-height:23px;'>" + department.InternalEmail + "</span></span><br/>";
                        }
                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.PNumber)
                        {
                            body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-PNumber", user.LanguageCode) + ": <span style='line-height:23px;'>" + department.DepartmentNumber + "</span></span><br/>";
                        }

                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.Account)
                        {
                            body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-Account", user.LanguageCode) + ":<span style='line-height:23px;'>" + field.CaseValue + "</span></span><br/>";
                        }
                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.PartnerId)
                        {
                            body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-PartnerId", user.LanguageCode) + ":<span style='line-height:23px;'>" + field.CaseValue + "</span></span><br/>"; // ERR?: duplicate
                        }
                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.SupplierCustomerNumber)
                        {
                            var supplierCustomerNumberModel = department.SupplierCustomerNumbers.LastOrDefault(scn => scn.SupplierId == supplier.SupplierId);
                            var supplierCnLabel = supplier.SupplierCaseFields.FirstOrDefault(d => d.CustomLabel != null);
                            if (supplierCnLabel != null && supplierCustomerNumberModel != null)
                            {
                                body += "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-Q8", user.LanguageCode) + ":<span style='line-height:23px;'>" + supplierCustomerNumberModel.SupplierCustomerId + "</span></span><br />";

                            }
                            else
                            {
                                body += supplierCustomerNumberModel != null ? "<span style='line-height:23px;'>" + _translationRepository.GetTranslation("CaseEmail-SCN", user.LanguageCode) + ":<span style='line-height:23px;'>" + supplierCustomerNumberModel.SupplierCustomerId + "</span></span><br />" : "";
                            }

                        }
                        else if (field.CaseField.FieldValue == SupplierLoginRequestField.PossibleBonusModel)
                        {
                            string savings = string.Empty;
                            body += "<span style='line-height:23px;'>" + GetPotentialSaving(member, department, supplier, user, out savings) + "</span>";
                            body += "<br/><span style='line-height:23px;'>" + _translationRepository.GetTranslation("Case-AverageConsumption", user.LanguageCode) + "<span style='line-height:23px;'>: " + savings + "</span></span><br />";

                        }

                    }
                }
            }

            body += "<br /><h3 style='line-height:22px;'>" + _translationRepository.GetTranslation("CaseEmail-OrderDetail", user.LanguageCode) + "</h3><br /><table class='table-bordered' id ='tbl' cellpadding='7' cellspacing='0' border = '1' >{0}</table>";
            return body;
        }

        public string GetPotentialSaving(Customer customer, Department department, Supplier supplier, AspNetUser user, out string savings)
        {
            // whole complex logic and service calls removed to improve readability and not dispose crucial code.
            // there were more possible lines added that contained various information depending of supplier agreement and what was ordered

            savings = "";
            return "";
        }

    }
}