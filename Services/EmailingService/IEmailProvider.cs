namespace EmailingService
{
    using MeetUp.Enumerations;

    public interface IEmailProvider
    {
        EmailProvider.EmailType GetTemplateType(string resource, LanguageCode culture, int? myChannel = null);

        string GetTemplateName(EmailProvider.EmailType emailType);

        void SetChannelId(int channelId);
    }
}