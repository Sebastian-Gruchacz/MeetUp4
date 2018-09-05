namespace EmailingService
{
    using System.Collections.Generic;

    using MeetUp.Model;

    public interface IMailMessageService
    {
        /// <summary>
        /// Saves Email Message Meta-data to DB. With attachments?
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        MailMessageResponse SaveMailMessage(MailMessage mailMessage);

        /// <summary>
        /// Get's list of messages connected to a lead. There may be whole stack of correspondence between Customer, supplier and Internal Support regarding.
        /// </summary>
        /// <param name="leadid"></param>
        /// <returns></returns>
        /// <remarks>Yes, name of the method is wrong.</remarks>
        List<MailMessage> GetParentLeadMessage(int leadid);

        /// <summary>
        /// Yes, another was yo save message into DB, this time outgoing message...
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="slug"></param>
        /// <param name="channelId"></param>
        /// <param name="emailContents"></param>
        /// <returns></returns>
        MailMessageResponse SaveMailMessage(MailMessage mail, string slug, string channelId, Dictionary<string, string> emailContents);
    }
}