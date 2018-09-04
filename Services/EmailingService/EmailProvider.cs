namespace EmailingService
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Mandrill;

    using NLog;

    public class EmailProvider : IEmailProvider
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static MandrillApi _api;
        private string _displayName = "";
        private string _fromAddress = "";
        private string _subject = "";
        private string _ccAddress = "";
        private string _attachment = "";
        private int _channelId;

        #region Public methods

        public EmailProvider()
        {
            _api = new MandrillApi(ConfigurationManager.AppSettings["MandrillKey"], false);
        }

        public EmailProvider(string channelId) : this()
        {
            this._channelId = Convert.ToInt32(channelId);
        }

        public void SetChannelId(int channelId)
        {
            this._channelId = channelId;
        }

        public TemplateInfo GetTemplateInfo(string slugId)
        {
            var template = _api.TemplateInfo(slugId);
            if (template != null)
                return template;

            return null;
        }

        public TemplateInfo GetTemplateInfo(string slugId, Dictionary<string, string> payload)
        {
            var template = _api.TemplateInfo(slugId);
            if (template != null)
            {
                foreach (var d in payload)
                {
                    var ph = "*|" + d.Key + "|*";
                    var val = d.Value;
                    template.publish_code = template.publish_code.Replace(ph, val);
                }
                return template;
            }
            return null;
        }

        public ComplexResult Send(string targetEmail, Dictionary<string, string> payload, string templateSlugId)
        {
            try
            {
                List<EmailAddress> toEmailAddresses = new List<EmailAddress>();

                EmailAddress toEmailAddress = new EmailAddress();
                toEmailAddress.email = targetEmail;

                toEmailAddresses.Add(toEmailAddress);

                if (payload.TryGetValue("cc", out this._ccAddress))
                {
                    EmailAddress ccEmailAddress = new EmailAddress();
                    ccEmailAddress.email = this._ccAddress;
                    ccEmailAddress.type = "cc";

                    toEmailAddresses.Add(ccEmailAddress);
                }

                EmailMessage emailMessage = new EmailMessage();
                emailMessage.to = toEmailAddresses;
                //   emailMessage.merge_language = "handlebars";
                emailMessage.merge = true;

                foreach (KeyValuePair<string, string> templateContent in payload)
                {
                    emailMessage.AddGlobalVariable(templateContent.Key, templateContent.Value);
                }

                if (payload.TryGetValue("from", out this._fromAddress))
                {
                    emailMessage.from_email = this._fromAddress;
                }
                if (payload.TryGetValue("from_name", out this._displayName))
                {
                    emailMessage.from_name = this._displayName;
                }
                if (payload.TryGetValue("subject", out this._subject))
                {
                    emailMessage.subject = this._subject;
                }

                if (payload.TryGetValue("attachment", out this._attachment))
                {
                    emailMessage.attachments = new List<email_attachment>();
                    email_attachment mailAttachment = new email_attachment();
                    if (this._attachment != null)
                    {
                        mailAttachment.content = this._attachment;
                        mailAttachment.name = "File.txt";
                        mailAttachment.type = "text/txt";
                        ((List<email_attachment>)emailMessage.attachments).Add(mailAttachment);
                    }
                }

                List<EmailResult> results = _api.SendMessage(emailMessage, templateSlugId, null);

                if (results.Count > 0)
                {

                    foreach (EmailResult result in results)
                    {
                        if (result.Status == EmailResultStatus.Sent || result.Status == EmailResultStatus.Queued)
                            return true;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return false;
            }
        }

        public ComplexResult Send(string targetEmail, Dictionary<string, string> payload, string templateSlugId, List<email_attachment> attachmentList)
        {
            try
            {
                if (payload == null)
                {
                    payload = new Dictionary<string, string>();
                }

                List<EmailAddress> toEmailAddresses = new List<EmailAddress>();

                EmailAddress toEmailAddress = new EmailAddress();
                toEmailAddress.email = targetEmail;

                toEmailAddresses.Add(toEmailAddress);

                if (payload.TryGetValue("cc", out this._ccAddress))
                {
                    EmailAddress ccEmailAddress = new EmailAddress();
                    ccEmailAddress.email = this._ccAddress;
                    ccEmailAddress.type = "cc";

                    toEmailAddresses.Add(ccEmailAddress);
                }

                EmailMessage emailMessage = new EmailMessage();
                emailMessage.to = toEmailAddresses;
                //   emailMessage.merge_language = "handlebars";
                emailMessage.merge = true;

                foreach (KeyValuePair<string, string> templateContent in payload)
                {
                    emailMessage.AddGlobalVariable(templateContent.Key, templateContent.Value);
                }

                if (payload.TryGetValue("from", out this._fromAddress))
                {
                    emailMessage.from_email = this._fromAddress;
                }
                if (payload.TryGetValue("from_name", out this._displayName))
                {
                    emailMessage.from_name = this._displayName;
                }
                if (payload.TryGetValue("subject", out this._subject))
                {
                    emailMessage.subject = this._subject;
                }

                if (attachmentList != null && attachmentList.Count > 0)
                {
                    emailMessage.attachments = attachmentList;
                }

                List<EmailResult> results = _api.SendMessage(emailMessage, templateSlugId, null);

                if (results.Count > 0)
                {
                    // TODO: partial success ?!?!
                    foreach (EmailResult result in results)
                    {
                        if (result.Status == EmailResultStatus.Sent || result.Status == EmailResultStatus.Queued)
                            return true;
                    }

                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return ex;
            }
        }

        public ComplexResult Send(string targetEmail, Dictionary<string, string> payload, EmailType emailType)
        {
            try
            {
                List<EmailAddress> toEmailAddresses = new List<EmailAddress>();
                //List<TemplateContent> contents = new List<TemplateContent>();

                EmailAddress toEmailAddress = new EmailAddress();
                toEmailAddress.email = targetEmail;

                toEmailAddresses.Add(toEmailAddress);

                if (payload.TryGetValue("cc", out this._ccAddress))
                {
                    EmailAddress ccEmailAddress = new EmailAddress();
                    ccEmailAddress.email = this._ccAddress;
                    ccEmailAddress.type = "cc";

                    toEmailAddresses.Add(ccEmailAddress);
                }

                EmailMessage emailMessage = new EmailMessage();
                emailMessage.to = toEmailAddresses;

                emailMessage.merge = true;

                foreach (KeyValuePair<string, string> templateContent in payload)
                {
                    emailMessage.AddGlobalVariable(templateContent.Key, templateContent.Value);
                }

                if (payload.TryGetValue("from", out this._fromAddress))
                {
                    emailMessage.from_email = this._fromAddress;
                }
                if (payload.TryGetValue("from_name", out this._displayName))
                {
                    emailMessage.from_name = this._displayName;
                }
                if (payload.TryGetValue("subject", out this._subject))
                {
                    emailMessage.subject = this._subject;
                }

                if (payload.TryGetValue("attachment", out this._attachment))
                {
                    //AttachmentCollection
                    emailMessage.attachments = new List<email_attachment>();
                    email_attachment mailAttachment = new email_attachment();
                    if (this._attachment != null)
                    {
                        mailAttachment.content = this._attachment;
                        mailAttachment.name = "File.txt";
                        mailAttachment.type = "text/txt";
                        ((List<email_attachment>)emailMessage.attachments).Add(mailAttachment);
                    }
                }

                //emailMessage.from_email = "info@justtcustomer.com";
                //emailMessage.subject = "Test";

                List<EmailResult> results = _api.SendMessage(emailMessage, this.GetTemplateName(emailType), null);

                if (results.Count > 0)
                {
                    foreach (EmailResult result in results)
                    {
                        if (result.Status == EmailResultStatus.Sent || result.Status == EmailResultStatus.Queued)
                            return true;
                    }

                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);

                var ms = ex.Message;
                if (ex.InnerException != null)
                    ms += ex.InnerException.Message;
                File.AppendAllText("C:\\Logs\\EmailProvider.txt", ms);
                return ex;
            }
        }

        #region send with Attachment

        public bool Send(string targetEmail, Dictionary<string, string> payload, EmailType emailType, byte[] byteArray)
        {
            List<EmailAddress> toEmailAddresses = new List<EmailAddress>();
            //List<TemplateContent> contents = new List<TemplateContent>();

            EmailAddress toEmailAddress = new EmailAddress();
            toEmailAddress.email = targetEmail;

            toEmailAddresses.Add(toEmailAddress);

            if (payload.TryGetValue("cc", out this._ccAddress))
            {
                EmailAddress ccEmailAddress = new EmailAddress();
                ccEmailAddress.email = this._ccAddress;
                ccEmailAddress.type = "cc";

                toEmailAddresses.Add(ccEmailAddress);
            }

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.to = toEmailAddresses;
            if (payload.TryGetValue("subject", out this._subject))
            {
                emailMessage.subject = this._subject;
            }
            emailMessage.attachments = new List<email_attachment>();
            email_attachment mailAttachment = new email_attachment();
            mailAttachment.content = Convert.ToBase64String(byteArray);
            mailAttachment.name = payload["AttachFileName"];//"PaymentLinkRequest.csv";
            mailAttachment.type = payload["AttachFileType"];
            ((List<email_attachment>)emailMessage.attachments).Add(mailAttachment);

            emailMessage.merge = true;

            foreach (KeyValuePair<string, string> templateContent in payload)
            {
                emailMessage.AddGlobalVariable(templateContent.Key, templateContent.Value);
            }

            //  emailMessage.from_email = FromAddress;
            //  emailMessage.subject = "Test";
            List<EmailResult> results = _api.SendMessage(emailMessage, this.GetTemplateName(emailType), null);


            if (results.Count > 0)
            {

                foreach (EmailResult result in results)
                {
                    if (result.Status == EmailResultStatus.Sent || result.Status == EmailResultStatus.Queued)
                        return true;
                }

            }

            return false;
        }

        public bool Send(string targetEmail, Dictionary<string, string> payload, EmailType emailType, List<email_attachment> attachmentList = null)
        {
            try
            {
                List<EmailAddress> toEmailAddresses = new List<EmailAddress>();
                //List<TemplateContent> contents = new List<TemplateContent>();

                EmailAddress toEmailAddress = new EmailAddress();
                toEmailAddress.email = targetEmail;

                toEmailAddresses.Add(toEmailAddress);

                if (payload.TryGetValue("cc", out this._ccAddress))
                {
                    EmailAddress ccEmailAddress = new EmailAddress();
                    ccEmailAddress.email = this._ccAddress;
                    ccEmailAddress.type = "cc";

                    toEmailAddresses.Add(ccEmailAddress);
                }

                EmailMessage emailMessage = new EmailMessage();
                emailMessage.to = toEmailAddresses;

                emailMessage.merge = true;

                foreach (KeyValuePair<string, string> templateContent in payload)
                {
                    emailMessage.AddGlobalVariable(templateContent.Key, templateContent.Value);
                }

                if (payload.TryGetValue("from", out this._fromAddress))
                {
                    emailMessage.from_email = this._fromAddress;
                }
                if (payload.TryGetValue("from_name", out this._displayName))
                {
                    emailMessage.from_name = this._displayName;
                }
                if (payload.TryGetValue("subject", out this._subject))
                {
                    emailMessage.subject = this._subject;
                }

                if (attachmentList != null && attachmentList.Count > 0)
                {
                    emailMessage.attachments = attachmentList;
                }

                //emailMessage.from_email = "info@justtcustomer.com";
                //emailMessage.subject = "Test";

                List<EmailResult> results = _api.SendMessage(emailMessage, this.GetTemplateName(emailType), null);


                if (results.Count > 0)
                {

                    foreach (EmailResult result in results)
                    {
                        if (result.Status == EmailResultStatus.Sent || result.Status == EmailResultStatus.Queued)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);

                return false;
            }
        }

        #endregion

        public enum EmailType
        {
            #region JUSTT

            JusttConfirmEn,
            JusttResetPasswordEn,
            JusttConfirmCategoryPersonEn,
            JusttConfirmExistingCategoryPersonEn,
            JusttAdminOfNewCompanyEn,
            JusttCustomerOrderEmailEn,
            JusttArvatoAccountRequiredEn,
            JusttUserEmailEn,
            JusttSupplierLoginRequestEn,
            JusttNewActityEn,
            JusttNewMessageEn,
            JusttAssignNewAdminEn,
            JusttAssignExistingAdminEn,
            JusttOfferRequestOrderEn,
            JusttOfferRequestOrderDa,
            JusttConfirmDa,
            JusttResetPasswordDa,
            JusttConfirmCategoryPersonDa,
            JusttConfirmExistingCategoryPersonDa,
            JusttAdminOfNewCompanyDa,
            JusttCustomerOrderEmailDa,
            JusttArvatoAccountRequiredDa,
            JusttUserEmailDa,
            JusttSupplierLoginRequestDa,
            JusttLoginRequestToAdmin,
            JusttNewActityDa,
            JusttNewMessageDa,
            JusttAssignNewAdminDa,
            JusttAssignExistingAdminDa,
            JusttMessageForward,
            JusttInviteUserInCompanyEn,
            JusttInviteUserInCompanyDa,
            JusttInviteUserInPlatformEn,
            JusttInviteUserInPlatformDa,
            JusttBonusLinesDa,
            JusttUpdateUserInCompnayEn,
            JusttUpdateUserInCompanyDa,
            JusttAdminInvitesUsersEn,
            JusttAdminInvitesUsersDa,
            JusttAdminNoInvitesUsersEn,
            JusttAdminNoInvitesUsersDa,
            #endregion

            #region DI

            DIConfirmEN,
            DIResetPasswordEN,
            DIConfirmCategoryPersonEN,
            DIConfirmExistingCategoryPersonEN,
            DIAdminOfNewCompanyEN,
            DICustomerOrderEmailEN,
            DICustomerOrderedEmailEN,
            DIArvatoAccountRequiredEN,
            DIUserEmailEN,
            DISupplierLoginRequestEN,
            DINewActityEN,
            DINewMessageEN,
            DIAssignNewAdminEN,
            DIAssignExistingAdminEN,
            DIOfferRequestOrderEN,
            DIOfferRequestOrderDA,
            DIConfirmDA,
            DIResetPasswordDA,
            DIConfirmCategoryPersonDA,
            DIConfirmExistingCategoryPersonDA,
            DIAdminOfNewCompanyDA,
            DICustomerOrderEmailDA,
            DIArvatoAccountRequiredDA,
            DIUserEmailDA,
            DISupplierLoginRequestDA,
            DINewActityDA,
            DINewMessageDA,
            DIAssignNewAdminDA,
            DIAssignExistingAdminDA,
            DIMessageForward,
            DIInviteUserInCompanyEN,
            DIInviteUserInCompanyDA,
            DIInviteUserInPlatformEN,
            DIInviteUserInPlatformDA,
            DIBonusLinesDA,
            DITermination,

            #endregion

            ArvatoPaymentLinks
        }

        #endregion

        #region Private Methods

        public string GetTemplateName(EmailType emailType)
        {
            switch (emailType)
            {
                #region JUSTT

                case EmailType.JusttBonusLinesDa:
                    return "justt-bonus-payout-mail-with-settlement";
                case EmailType.JusttConfirmEn:
                    return "justt-welcome-user-en";
                case EmailType.JusttConfirmDa:
                    return "justt-welcome-user-da-new";

                case EmailType.JusttResetPasswordEn:
                    return "justt-forgot-password-en";
                case EmailType.JusttResetPasswordDa:
                    return "justt-forgot-password-da-new";

                case EmailType.JusttAdminOfNewCompanyEn:
                    return "justt-admin-of-new-company-en";
                case EmailType.JusttAdminOfNewCompanyDa:
                    return "justt-admin-of-new-company-da-new";

                case EmailType.JusttAssignNewAdminEn:
                    return "justt-assign-admin-notification-new-user-en";
                case EmailType.JusttAssignNewAdminDa:
                    return "justt-assign-admin-notification-new-user-da";

                case EmailType.JusttAssignExistingAdminEn:
                    return "justt-assign-admin-notification-existing-user-en";
                case EmailType.JusttAssignExistingAdminDa:
                    return "justt-assign-admin-notification-existing-user-da";

                case EmailType.JusttConfirmCategoryPersonEn:
                    return "justt-assign-person-notification-new-user-en";
                case EmailType.JusttConfirmCategoryPersonDa:
                    return "justt-assign-person-notification-new-user-da";

                case EmailType.JusttConfirmExistingCategoryPersonEn:
                    return "justt-assign-person-notification-existing-user-en";
                case EmailType.JusttConfirmExistingCategoryPersonDa:
                    return "justt-assign-person-notification-existing-user-da";

                case EmailType.JusttCustomerOrderEmailEn:
                    return "justt-customer-order-en";
                case EmailType.JusttCustomerOrderEmailDa:
                    return "justt-customer-order-da";

                case EmailType.JusttArvatoAccountRequiredEn:
                    return "justt-supplier-activation-en";
                case EmailType.JusttArvatoAccountRequiredDa:
                    return "justt-supplier-activation-en";

                case EmailType.JusttUserEmailEn:
                    return "justt-simple-message-en";
                case EmailType.JusttUserEmailDa:
                    return "justt-simple-message-en";

                case EmailType.JusttNewActityEn:
                    return "justt-new-activity-en";
                case EmailType.JusttNewActityDa:
                    return "justt-new-activity-da";

                case EmailType.JusttNewMessageEn:
                    return "justt-new-message-en";
                case EmailType.JusttNewMessageDa:
                    return "justt-new-message-da-new";

                case EmailType.JusttSupplierLoginRequestEn:
                    return "justt-supplier-activation-en";
                case EmailType.JusttSupplierLoginRequestDa:
                    return "justt-supplier-activation-da";

                case EmailType.JusttOfferRequestOrderEn:
                    return "justt-offer-request-or-order-placed-en-v2";
                case EmailType.JusttOfferRequestOrderDa:
                    return "justt-offer-request-or-order-placed-da-v2-new";

                case EmailType.JusttLoginRequestToAdmin:
                    return "justt-login-request-to-admin";

                case EmailType.JusttMessageForward:
                    return "justt-message-forward";

                //case EmailType.JusttInviteUserInCompanyEN:
                //    return "justt-invite-admin-uk";

                case EmailType.JusttInviteUserInCompanyEn:
                case EmailType.JusttInviteUserInCompanyDa:
                    return "justt-invite-admin-dk-new";

                //case EmailType.JusttInviteUserInPlatformEN:
                //    return "justt-invite-new-user-en";

                case EmailType.JusttInviteUserInPlatformEn:
                case EmailType.JusttInviteUserInPlatformDa:
                    return "justt-invite-new-user-da-new";
                case EmailType.JusttUpdateUserInCompanyDa:
                case EmailType.JusttUpdateUserInCompnayEn:
                    return "justt-invite-admin-dk-new";

                case EmailType.JusttAdminNoInvitesUsersEn:
                case EmailType.JusttAdminNoInvitesUsersDa:
                    return "justt-welcome-user-no-invites-new";

                case EmailType.JusttAdminInvitesUsersEn:
                case EmailType.JusttAdminInvitesUsersDa:
                    return "justt-welcome-user-invites-new";

                #endregion

                #region DI
                case EmailType.DITermination:
                    return "di-termination-mail-bonus";

                case EmailType.DIBonusLinesDA:
                    return "di-bonus-payout-mail-with-settlement";

                case EmailType.DIConfirmDA:
                case EmailType.DIConfirmEN:
                    return "di-welcome-user-da";

                case EmailType.DIResetPasswordDA:
                case EmailType.DIResetPasswordEN:
                    return "di-forgot-password-da";

                case EmailType.DIAdminOfNewCompanyDA:
                case EmailType.DIAdminOfNewCompanyEN:
                    return "di-admin-of-new-company-da";

                case EmailType.DIAssignNewAdminEN:
                    return "di-assign-admin-notification-new-user-da";
                case EmailType.DIAssignNewAdminDA:
                    return "di-assign-admin-notification-new-user-da";

                case EmailType.DIAssignExistingAdminEN:
                    return "di-assign-admin-notification-existing-user-da";
                case EmailType.DIAssignExistingAdminDA:
                    return "di-assign-admin-notification-existing-user-da";

                case EmailType.DIConfirmCategoryPersonDA:
                case EmailType.DIConfirmCategoryPersonEN:
                    return "di-assign-person-notification-new-user-da";

                case EmailType.DIConfirmExistingCategoryPersonDA:
                case EmailType.DIConfirmExistingCategoryPersonEN:
                    return "di-assign-person-notification-existing-user-da";

                case EmailType.DICustomerOrderEmailDA:
                case EmailType.DICustomerOrderEmailEN:
                    return "di-customer-order-da";

                case EmailType.DIArvatoAccountRequiredDA:
                case EmailType.DIArvatoAccountRequiredEN:
                    return "di-arvato-account-customer-da";

                case EmailType.DIUserEmailDA:
                case EmailType.DIUserEmailEN:
                    return "di-simple-message-da";

                case EmailType.DINewActityDA:
                case EmailType.DINewActityEN:
                    return "di-new-activity-da";

                case EmailType.DINewMessageEN:
                case EmailType.DINewMessageDA:
                    return "di-new-message-da";

                case EmailType.DISupplierLoginRequestDA:
                case EmailType.DISupplierLoginRequestEN:
                    return "di-supplier-activation-da";

                case EmailType.DIOfferRequestOrderEN:
                case EmailType.DIOfferRequestOrderDA:
                    return "di-offer-request-or-order-placed-da-v2";

                case EmailType.DIMessageForward:
                    return "di-message-forward";

                case EmailType.DIInviteUserInCompanyEN:
                case EmailType.DIInviteUserInCompanyDA:
                    return "di-invite-admin-da";

                case EmailType.DIInviteUserInPlatformEN:
                case EmailType.DIInviteUserInPlatformDA:
                    return "di-invite-new-user-da";

                #endregion

                default:
                    return "di-simple-message-da";
            }
        }

        public EmailType GetTemplateType(string resource, string culture, int? myChannel = null)
        {
            if (myChannel != null)
                this._channelId = (int)myChannel;
            switch (resource)
            {
                #region Confirm
                case "Confirm":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttConfirmEn;
                                default:
                                    return EmailType.JusttConfirmDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIConfirmDA;
                                default:
                                    return EmailType.DIConfirmDA;
                            }
                    }
                    break;
                #endregion

                #region Reset
                case "Reset":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttResetPasswordEn;
                                default:
                                    return EmailType.JusttResetPasswordDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIResetPasswordDA;
                                default:
                                    return EmailType.DIResetPasswordDA;
                            }
                    }
                    break;
                #endregion

                #region NewAssignUser
                case "NewAssignUser":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttConfirmCategoryPersonEn;
                                default:
                                    return EmailType.JusttConfirmCategoryPersonDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIConfirmCategoryPersonDA;
                                default:
                                    return EmailType.DIConfirmCategoryPersonDA;
                            }
                    }
                    break;
                #endregion

                #region CustomerOrder
                case "CustomerOrder":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttCustomerOrderEmailEn;
                                default:
                                    return EmailType.JusttCustomerOrderEmailDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DICustomerOrderEmailDA;
                                default:
                                    return EmailType.DICustomerOrderEmailDA;
                            }
                    }
                    break;
                #endregion

                #region ArvatoAccount
                case "ArvatoAccount":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttArvatoAccountRequiredEn;
                                default:
                                    return EmailType.DIArvatoAccountRequiredDA;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIArvatoAccountRequiredDA;
                                default:
                                    return EmailType.DIArvatoAccountRequiredDA;
                            }
                    }
                    break;
                #endregion

                #region NewCompany
                case "NewCompany":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttAdminOfNewCompanyEn;
                                default:
                                    return EmailType.JusttAdminOfNewCompanyDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIAdminOfNewCompanyDA;
                                default:
                                    return EmailType.DIAdminOfNewCompanyDA;
                            }
                    }
                    break;
                #endregion

                #region SupplierLoginRequest
                case "SupplierLoginRequest":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttSupplierLoginRequestEn;
                                default:
                                    return EmailType.JusttSupplierLoginRequestDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DISupplierLoginRequestDA;
                                default:
                                    return EmailType.DISupplierLoginRequestDA;
                            }
                    }
                    break;
                #endregion

                #region UserEmail
                case "UserEmailCommunication":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttUserEmailEn;
                                default:
                                    return EmailType.JusttUserEmailDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIUserEmailDA;
                                default:
                                    return EmailType.DIUserEmailDA;
                            }
                    }
                    break;
                #endregion

                #region OfferRequestOrder
                case "OfferRequestOrder":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttOfferRequestOrderEn;
                                default:
                                    return EmailType.JusttOfferRequestOrderDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIOfferRequestOrderEN;
                                default:
                                    return EmailType.DIOfferRequestOrderDA;
                            }
                    }
                    break;
                #endregion

                #region InviteUserInCompany
                case "InviteUserInCompany":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttInviteUserInCompanyEn;
                                default:
                                    return EmailType.JusttInviteUserInCompanyDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIInviteUserInCompanyEN;
                                default:
                                    return EmailType.DIInviteUserInCompanyDA;
                            }
                    }
                    break;
                #endregion

                #region InviteUserInPlatform
                case "InviteUserInPlatform":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttInviteUserInPlatformEn;
                                default:
                                    return EmailType.JusttInviteUserInPlatformDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIInviteUserInPlatformEN;
                                default:
                                    return EmailType.DIInviteUserInPlatformDA;
                            }
                    }
                    break;
                #endregion

                #region InviteUserInPlatform
                case "UpdateUserInCompany":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttUpdateUserInCompnayEn;
                                default:
                                    return EmailType.JusttUpdateUserInCompanyDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIInviteUserInCompanyEN;
                                default:
                                    return EmailType.DIInviteUserInCompanyDA;
                            }
                    }
                    break;
                #endregion

                #region BonusLines
                case "BonusLines":
                    switch (this._channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.JusttBonusLinesDa;
                                default:
                                    return EmailType.JusttBonusLinesDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case "en-US":
                                    return EmailType.DIBonusLinesDA;
                                default:
                                    return EmailType.DIBonusLinesDA;
                            }
                    }
                    break;
                #endregion

                #region AdminInviteUserEasyOnboarding
                case "AdminInviteUserEasyOnboarding":
                    return EmailType.JusttAdminInvitesUsersDa;
                    break;
                #endregion

                #region AdminNoInviteUserEasyOnboarding
                case "AdminNoInviteUserEasyOnboarding":
                    return EmailType.JusttAdminNoInvitesUsersDa;
                    break;
                #endregion

            }

            return EmailType.JusttConfirmEn;
        }
        #endregion
    }

    
}