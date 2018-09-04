namespace OrderService
{
    using System.Collections.Generic;

    using MeetUp.Model;

    public interface IMailMessageService
    {
        MailMessageResponse SaveMailMessage(MailMessage mailMessage);
        List<MailMessage> GetParentLeadMessage(int leadid);
        void SaveMailMessage(MailMessage mail, string slug, string channelId, Dictionary<string, string> emailContents);
    }
}