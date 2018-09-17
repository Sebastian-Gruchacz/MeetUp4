namespace OrderFormService.OtherServices
{
    using MeetUp.Enumerations;

    public interface ITranslationsRepository
    {
        string GetTranslation(string translationKey, LanguageCode languageCode);
    }
}