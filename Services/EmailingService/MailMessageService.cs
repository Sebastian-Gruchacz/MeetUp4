namespace EmailingService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MeetUp.BLL;
    using MeetUp.Enumerations;
    using MeetUp.Model;

    using NLog;

    public class MailMessageService : IMailMessageService
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMailMessageRepository _mailMessageRepository;

        public MailMessageService(IMailMessageRepository mailMessageRepository)
        {
            _mailMessageRepository = mailMessageRepository;
        }

        /// <summary>
        /// To save Mail Message.
        /// </summary>
        /// <param name="mailMessage">Mail Message Object</param>
        /// <returns>Response Object</returns>
        public MailMessageResponse SaveMailMessage(MailMessage mailMessage)
        {
            if (mailMessage != null)
            {
                if (mailMessage.MessageId != Guid.Empty)
                {
                    MailMessage messageModel = _mailMessageRepository.All.FirstOrDefault(i => i.MessageId == mailMessage.MessageId);

                    if (messageModel != null)
                    {
                        messageModel.Status = MessageStatus.Unread;
                        messageModel.SentUtcDateTime = DateTime.UtcNow;

                        _mailMessageRepository.Update(messageModel);
                        _mailMessageRepository.SaveChanges();

                        return new MailMessageResponse
                        {
                            IsSuccess = true,
                            MessageId = messageModel.MessageId,
                            Message = ResponseMessage.RecordUpdated
                        };
                    }
                }

                mailMessage.MessageId = Guid.NewGuid();

                mailMessage.Status = MessageStatus.Unread;
                mailMessage.SentUtcDateTime = DateTime.UtcNow;

                _mailMessageRepository.Insert(mailMessage);
                _mailMessageRepository.SaveChanges();

                return new MailMessageResponse
                {
                    IsSuccess = true,
                    MessageId = mailMessage.MessageId,
                    Message = ResponseMessage.RecordSaved
                };
            }

            return new MailMessageResponse
            {
                IsSuccess = false,
                MessageId = Guid.Empty,
                Message = ResponseMessage.InvalidParam
            };
        }

        public List<MailMessage> GetParentLeadMessage(int trackingId)
        {
            return _mailMessageRepository.All.Where(i => i.LeadTrackingId == trackingId && i.ParentMessageId == null).ToList();
        }

        public MailMessageResponse SaveMailMessage(MailMessage model, string template, string channelId, Dictionary<string, string> data)
        {
            EmailProvider client = new EmailProvider(channelId);
            var templateInfo = client.GetTemplateInfo(template, data);

            if (model != null && templateInfo != null)
            {
                model.MessageId = Guid.NewGuid();
                model.Status = MessageStatus.Unread;

                model.SentUtcDateTime = DateTime.UtcNow;
                model.Body = templateInfo.publish_code;

                if (string.IsNullOrEmpty(model.Subject))
                    model.Subject = templateInfo.PublishSubject;

                if (string.IsNullOrEmpty(model.FromAddress))
                    model.FromAddress = templateInfo.PublishFromEmail;

                if (string.IsNullOrEmpty(model.DisplayName))
                    model.DisplayName = templateInfo.PublishFromName;

                _mailMessageRepository.Insert(model);
                _mailMessageRepository.SaveChanges();

                return new MailMessageResponse
                {
                    IsSuccess = true,
                    MessageId = model.MessageId,
                    Message = ResponseMessage.RecordSaved
                };
            }

            return new MailMessageResponse
            {
                IsSuccess = false,
                MessageId = Guid.Empty,
                Message = ResponseMessage.InvalidParam
            };
        }
    }


}