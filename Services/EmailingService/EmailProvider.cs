namespace EmailingService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Mandrill;
    using Mandrill.Models;

    using MeetUp.Enumerations;

    using NLog;

    public class EmailProvider : IEmailProvider
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static MandrillApi _api;
        private string _displayName = "";
        private string _fromAddress = "";
        private string _subject = "";
        private string _ccAddress = "";
        private readonly string _attachment = "";
        private int _channelId;

        #region Public methods

        public EmailProvider()
        {
            _api = new MandrillApi(ConfigurationManager.AppSettings["MandrillKey"], false);
        }

        public EmailProvider(string channelId) : this()
        {
            _channelId = Convert.ToInt32(channelId);
        }

        public void SetChannelId(int channelId)
        {
            _channelId = channelId;
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

        #region send with Attachment

        public bool Send(string targetEmail, Dictionary<string, string> payload, EmailType emailType, byte[] byteArray)
        {
            List<EmailAddress> toEmailAddresses = new List<EmailAddress>();
            //List<TemplateContent> contents = new List<TemplateContent>();

            EmailAddress toEmailAddress = new EmailAddress();
            toEmailAddress.email = targetEmail;

            toEmailAddresses.Add(toEmailAddress);

            if (payload.TryGetValue("cc", out _ccAddress))
            {
                EmailAddress ccEmailAddress = new EmailAddress();
                ccEmailAddress.email = _ccAddress;
                ccEmailAddress.type = "cc";

                toEmailAddresses.Add(ccEmailAddress);
            }

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.to = toEmailAddresses;
            if (payload.TryGetValue("subject", out _subject))
            {
                emailMessage.subject = _subject;
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
            List<EmailResult> results = _api.SendMessage(emailMessage, GetTemplateName(emailType), null);


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

                if (payload.TryGetValue("cc", out _ccAddress))
                {
                    EmailAddress ccEmailAddress = new EmailAddress();
                    ccEmailAddress.email = _ccAddress;
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

                if (payload.TryGetValue("from", out _fromAddress))
                {
                    emailMessage.from_email = _fromAddress;
                }
                if (payload.TryGetValue("from_name", out _displayName))
                {
                    emailMessage.from_name = _displayName;
                }
                if (payload.TryGetValue("subject", out _subject))
                {
                    emailMessage.subject = _subject;
                }

                if (attachmentList != null && attachmentList.Count > 0)
                {
                    emailMessage.attachments = attachmentList;
                }

                //emailMessage.from_email = "info@customer.com";
                //emailMessage.subject = "Test";

                List<EmailResult> results = _api.SendMessage(emailMessage, GetTemplateName(emailType), null);


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
            #region ChannelOne

            ChannelOneConfirmEn,
            ChannelOneResetPasswordEn,
            ChannelOneConfirmCategoryPersonEn,
            ChannelOneConfirmExistingCategoryPersonEn,
            ChannelOneAdminOfNewCompanyEn,
            ChannelOneCustomerOrderEmailEn,
            ChannelOneUserEmailEn,
            ChannelOneSupplierLoginRequestEn,
            ChannelOneNewActityEn,
            ChannelOneNewMessageEn,
            ChannelOneAssignNewAdminEn,
            ChannelOneAssignExistingAdminEn,
            ChannelOneOfferRequestOrderEn,
            ChannelOneOfferRequestOrderDa,
            ChannelOneConfirmDa,
            ChannelOneResetPasswordDa,
            ChannelOneConfirmCategoryPersonDa,
            ChannelOneConfirmExistingCategoryPersonDa,
            ChannelOneAdminOfNewCompanyDa,
            ChannelOneCustomerOrderEmailDa,
            ChannelOneUserEmailDa,
            ChannelOneSupplierLoginRequestDa,
            ChannelOneLoginRequestToAdmin,
            ChannelOneNewActityDa,
            ChannelOneNewMessageDa,
            ChannelOneAssignNewAdminDa,
            ChannelOneAssignExistingAdminDa,
            ChannelOneMessageForward,
            ChannelOneInviteUserInCompanyEn,
            ChannelOneInviteUserInCompanyDa,
            ChannelOneInviteUserInPlatformEn,
            ChannelOneInviteUserInPlatformDa,
            ChannelOneBonusLinesDa,
            ChannelOneUpdateUserInCompnayEn,
            ChannelOneUpdateUserInCompanyDa,
            ChannelOneAdminInvitesUsersEn,
            ChannelOneAdminInvitesUsersDa,
            ChannelOneAdminNoInvitesUsersEn,
            ChannelOneAdminNoInvitesUsersDa,

            #endregion

            #region ChannelTwo

            ChannelTwoConfirmEN,
            ChannelTwoResetPasswordEN,
            ChannelTwoConfirmCategoryPersonEN,
            ChannelTwoConfirmExistingCategoryPersonEN,
            ChannelTwoAdminOfNewCompanyEN,
            ChannelTwoCustomerOrderEmailEN,
            ChannelTwoCustomerOrderedEmailEN,
            ChannelTwoUserEmailEN,
            ChannelTwoSupplierLoginRequestEN,
            ChannelTwoNewActityEN,
            ChannelTwoNewMessageEN,
            ChannelTwoAssignNewAdminEN,
            ChannelTwoAssignExistingAdminEN,
            ChannelTwoOfferRequestOrderEN,
            ChannelTwoOfferRequestOrderDA,
            ChannelTwoConfirmDA,
            ChannelTwoResetPasswordDA,
            ChannelTwoConfirmCategoryPersonDA,
            ChannelTwoConfirmExistingCategoryPersonDA,
            ChannelTwoAdminOfNewCompanyDA,
            ChannelTwoCustomerOrderEmailDA,
            ChannelTwoUserEmailDA,
            ChannelTwoSupplierLoginRequestDA,
            ChannelTwoNewActityDA,
            ChannelTwoNewMessageDA,
            ChannelTwoAssignNewAdminDA,
            ChannelTwoAssignExistingAdminDA,
            ChannelTwoMessageForward,
            ChannelTwoInviteUserInCompanyEN,
            ChannelTwoInviteUserInCompanyDA,
            ChannelTwoInviteUserInPlatformEN,
            ChannelTwoInviteUserInPlatformDA,
            ChannelTwoBonusLinesDA,
            ChannelTwoTermination,

            #endregion
        }

        #endregion

        #region Private Methods

        public string GetTemplateName(EmailType emailType)
        {
            switch (emailType)
            {
                #region ChannelOne

                case EmailType.ChannelOneBonusLinesDa:
                    return "ChannelOne-bonus-payout-mail-with-settlement";
                case EmailType.ChannelOneConfirmEn:
                    return "ChannelOne-welcome-user-en";
                case EmailType.ChannelOneConfirmDa:
                    return "ChannelOne-welcome-user-da-new";

                case EmailType.ChannelOneResetPasswordEn:
                    return "ChannelOne-forgot-password-en";
                case EmailType.ChannelOneResetPasswordDa:
                    return "ChannelOne-forgot-password-da-new";

                case EmailType.ChannelOneAdminOfNewCompanyEn:
                    return "ChannelOne-admin-of-new-company-en";
                case EmailType.ChannelOneAdminOfNewCompanyDa:
                    return "ChannelOne-admin-of-new-company-da-new";

                case EmailType.ChannelOneAssignNewAdminEn:
                    return "ChannelOne-assign-admin-notification-new-user-en";
                case EmailType.ChannelOneAssignNewAdminDa:
                    return "ChannelOne-assign-admin-notification-new-user-da";

                case EmailType.ChannelOneAssignExistingAdminEn:
                    return "ChannelOne-assign-admin-notification-existing-user-en";
                case EmailType.ChannelOneAssignExistingAdminDa:
                    return "ChannelOne-assign-admin-notification-existing-user-da";

                case EmailType.ChannelOneConfirmCategoryPersonEn:
                    return "ChannelOne-assign-person-notification-new-user-en";
                case EmailType.ChannelOneConfirmCategoryPersonDa:
                    return "ChannelOne-assign-person-notification-new-user-da";

                case EmailType.ChannelOneConfirmExistingCategoryPersonEn:
                    return "ChannelOne-assign-person-notification-existing-user-en";
                case EmailType.ChannelOneConfirmExistingCategoryPersonDa:
                    return "ChannelOne-assign-person-notification-existing-user-da";

                case EmailType.ChannelOneCustomerOrderEmailEn:
                    return "ChannelOne-customer-order-en";
                case EmailType.ChannelOneCustomerOrderEmailDa:
                    return "ChannelOne-customer-order-da";

                case EmailType.ChannelOneUserEmailEn:
                    return "ChannelOne-simple-message-en";
                case EmailType.ChannelOneUserEmailDa:
                    return "ChannelOne-simple-message-en";

                case EmailType.ChannelOneNewActityEn:
                    return "ChannelOne-new-activity-en";
                case EmailType.ChannelOneNewActityDa:
                    return "ChannelOne-new-activity-da";

                case EmailType.ChannelOneNewMessageEn:
                    return "ChannelOne-new-message-en";
                case EmailType.ChannelOneNewMessageDa:
                    return "ChannelOne-new-message-da-new";

                case EmailType.ChannelOneSupplierLoginRequestEn:
                    return "ChannelOne-supplier-activation-en";
                case EmailType.ChannelOneSupplierLoginRequestDa:
                    return "ChannelOne-supplier-activation-da";

                case EmailType.ChannelOneOfferRequestOrderEn:
                    return "ChannelOne-offer-request-or-order-placed-en-v2";
                case EmailType.ChannelOneOfferRequestOrderDa:
                    return "ChannelOne-offer-request-or-order-placed-da-v2-new";

                case EmailType.ChannelOneLoginRequestToAdmin:
                    return "ChannelOne-login-request-to-admin";

                case EmailType.ChannelOneMessageForward:
                    return "ChannelOne-message-forward";

                case EmailType.ChannelOneInviteUserInCompanyEn:
                case EmailType.ChannelOneInviteUserInCompanyDa:
                    return "ChannelOne-invite-admin-dk-new";

                case EmailType.ChannelOneInviteUserInPlatformEn:
                case EmailType.ChannelOneInviteUserInPlatformDa:
                    return "ChannelOne-invite-new-user-da-new";
                case EmailType.ChannelOneUpdateUserInCompanyDa:
                case EmailType.ChannelOneUpdateUserInCompnayEn:
                    return "ChannelOne-invite-admin-dk-new";

                case EmailType.ChannelOneAdminNoInvitesUsersEn:
                case EmailType.ChannelOneAdminNoInvitesUsersDa:
                    return "ChannelOne-welcome-user-no-invites-new";

                case EmailType.ChannelOneAdminInvitesUsersEn:
                case EmailType.ChannelOneAdminInvitesUsersDa:
                    return "ChannelOne-welcome-user-invites-new";

                #endregion

                #region ChannelTwo
                case EmailType.ChannelTwoTermination:
                    return "ChannelTwo-termination-mail-bonus";

                case EmailType.ChannelTwoBonusLinesDA:
                    return "ChannelTwo-bonus-payout-mail-with-settlement";

                case EmailType.ChannelTwoConfirmDA:
                case EmailType.ChannelTwoConfirmEN:
                    return "ChannelTwo-welcome-user-da";

                case EmailType.ChannelTwoResetPasswordDA:
                case EmailType.ChannelTwoResetPasswordEN:
                    return "ChannelTwo-forgot-password-da";

                case EmailType.ChannelTwoAdminOfNewCompanyDA:
                case EmailType.ChannelTwoAdminOfNewCompanyEN:
                    return "ChannelTwo-admin-of-new-company-da";

                case EmailType.ChannelTwoAssignNewAdminEN:
                    return "ChannelTwo-assign-admin-notification-new-user-da";
                case EmailType.ChannelTwoAssignNewAdminDA:
                    return "ChannelTwo-assign-admin-notification-new-user-da";

                case EmailType.ChannelTwoAssignExistingAdminEN:
                    return "ChannelTwo-assign-admin-notification-existing-user-da";
                case EmailType.ChannelTwoAssignExistingAdminDA:
                    return "ChannelTwo-assign-admin-notification-existing-user-da";

                case EmailType.ChannelTwoConfirmCategoryPersonDA:
                case EmailType.ChannelTwoConfirmCategoryPersonEN:
                    return "ChannelTwo-assign-person-notification-new-user-da";

                case EmailType.ChannelTwoConfirmExistingCategoryPersonDA:
                case EmailType.ChannelTwoConfirmExistingCategoryPersonEN:
                    return "ChannelTwo-assign-person-notification-existing-user-da";

                case EmailType.ChannelTwoCustomerOrderEmailDA:
                case EmailType.ChannelTwoCustomerOrderEmailEN:
                    return "ChannelTwo-customer-order-da";

                case EmailType.ChannelTwoUserEmailDA:
                case EmailType.ChannelTwoUserEmailEN:
                    return "ChannelTwo-simple-message-da";

                case EmailType.ChannelTwoNewActityDA:
                case EmailType.ChannelTwoNewActityEN:
                    return "ChannelTwo-new-activity-da";

                case EmailType.ChannelTwoNewMessageEN:
                case EmailType.ChannelTwoNewMessageDA:
                    return "ChannelTwo-new-message-da";

                case EmailType.ChannelTwoSupplierLoginRequestDA:
                case EmailType.ChannelTwoSupplierLoginRequestEN:
                    return "ChannelTwo-supplier-activation-da";

                case EmailType.ChannelTwoOfferRequestOrderEN:
                case EmailType.ChannelTwoOfferRequestOrderDA:
                    return "ChannelTwo-offer-request-or-order-placed-da-v2";

                case EmailType.ChannelTwoMessageForward:
                    return "ChannelTwo-message-forward";

                case EmailType.ChannelTwoInviteUserInCompanyEN:
                case EmailType.ChannelTwoInviteUserInCompanyDA:
                    return "ChannelTwo-invite-admin-da";

                case EmailType.ChannelTwoInviteUserInPlatformEN:
                case EmailType.ChannelTwoInviteUserInPlatformDA:
                    return "ChannelTwo-invite-new-user-da";

                #endregion

                default:
                    return "ChannelTwo-simple-message-da";
            }
        }

        public EmailType GetTemplateType(string resource, LanguageCode culture, int? myChannel = null)
        {
            if (myChannel != null)
                _channelId = (int)myChannel;

            switch (resource)
            {
                #region Confirm
                case "Confirm":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture.GetLanguageCodeString())
                            {
                                case @"en-US":                              // This is how whole method was...
                                    return EmailType.ChannelOneConfirmEn;
                                default:
                                    return EmailType.ChannelOneConfirmDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:                  // now a bit improved
                                    return EmailType.ChannelTwoConfirmDA;
                                default:
                                    return EmailType.ChannelTwoConfirmDA;
                            }
                    }
                    break;
                #endregion

                #region Reset
                case "Reset":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneResetPasswordEn;
                                default:
                                    return EmailType.ChannelOneResetPasswordDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoResetPasswordDA;
                                default:
                                    return EmailType.ChannelTwoResetPasswordDA;
                            }
                    }
                    break;
                #endregion

                #region NewAssignUser
                case "NewAssignUser":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneConfirmCategoryPersonEn;
                                default:
                                    return EmailType.ChannelOneConfirmCategoryPersonDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoConfirmCategoryPersonDA;
                                default:
                                    return EmailType.ChannelTwoConfirmCategoryPersonDA;
                            }
                    }
                    break;
                #endregion

                #region CustomerOrder
                case "CustomerOrder":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneCustomerOrderEmailEn;
                                default:
                                    return EmailType.ChannelOneCustomerOrderEmailDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoCustomerOrderEmailDA;
                                default:
                                    return EmailType.ChannelTwoCustomerOrderEmailDA;
                            }
                    }
                    break;
                #endregion


                #region NewCompany
                case "NewCompany":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneAdminOfNewCompanyEn;
                                default:
                                    return EmailType.ChannelOneAdminOfNewCompanyDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoAdminOfNewCompanyDA;
                                default:
                                    return EmailType.ChannelTwoAdminOfNewCompanyDA;
                            }
                    }
                    break;
                #endregion

                #region SupplierLoginRequest
                case "SupplierLoginRequest":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneSupplierLoginRequestEn;
                                default:
                                    return EmailType.ChannelOneSupplierLoginRequestDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoSupplierLoginRequestDA;
                                default:
                                    return EmailType.ChannelTwoSupplierLoginRequestDA;
                            }
                    }
                    break;
                #endregion

                #region UserEmail
                case "UserEmailCommunication":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneUserEmailEn;
                                default:
                                    return EmailType.ChannelOneUserEmailDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoUserEmailDA;
                                default:
                                    return EmailType.ChannelTwoUserEmailDA;
                            }
                    }
                    break;
                #endregion

                #region OfferRequestOrder
                case "OfferRequestOrder":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneOfferRequestOrderEn;
                                default:
                                    return EmailType.ChannelOneOfferRequestOrderDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoOfferRequestOrderEN;
                                default:
                                    return EmailType.ChannelTwoOfferRequestOrderDA;
                            }
                    }
                    break;
                #endregion

                #region InviteUserInCompany
                case "InviteUserInCompany":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneInviteUserInCompanyEn;
                                default:
                                    return EmailType.ChannelOneInviteUserInCompanyDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoInviteUserInCompanyEN;
                                default:
                                    return EmailType.ChannelTwoInviteUserInCompanyDA;
                            }
                    }
                    break;
                #endregion

                #region InviteUserInPlatform
                case "InviteUserInPlatform":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneInviteUserInPlatformEn;
                                default:
                                    return EmailType.ChannelOneInviteUserInPlatformDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoInviteUserInPlatformEN;
                                default:
                                    return EmailType.ChannelTwoInviteUserInPlatformDA;
                            }
                    }
                    break;
                #endregion

                #region InviteUserInPlatform
                case "UpdateUserInCompany":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneUpdateUserInCompnayEn;
                                default:
                                    return EmailType.ChannelOneUpdateUserInCompanyDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoInviteUserInCompanyEN;
                                default:
                                    return EmailType.ChannelTwoInviteUserInCompanyDA;
                            }
                    }
                    break;
                #endregion

                #region BonusLines
                case "BonusLines":
                    switch (_channelId)
                    {
                        case 1:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelOneBonusLinesDa;
                                default:
                                    return EmailType.ChannelOneBonusLinesDa;
                            }
                        case 12:
                            switch (culture)
                            {
                                case LanguageCode.English:
                                    return EmailType.ChannelTwoBonusLinesDA;
                                default:
                                    return EmailType.ChannelTwoBonusLinesDA;
                            }
                    }
                    break;
                #endregion

                #region AdminInviteUserEasyOnboarding

                case "AdminInviteUserEasyOnboarding":
                    return EmailType.ChannelOneAdminInvitesUsersDa;
                    break;

                #endregion

                #region AdminNoInviteUserEasyOnboarding

                case "AdminNoInviteUserEasyOnboarding":
                    return EmailType.ChannelOneAdminNoInvitesUsersDa;
                    break;

                    #endregion

            }

            return EmailType.ChannelOneConfirmEn;
        }

        #endregion
    }
}