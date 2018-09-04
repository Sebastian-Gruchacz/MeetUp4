namespace OrderService
{
    public interface IEmailProvider
    {
        EmailProvider.EmailType GetTemplateType(string resource, string culture, int? myChannel = null);

        string GetTemplateName(EmailProvider.EmailType emailType);

        void SetChannelId(int channelId);
    }
}