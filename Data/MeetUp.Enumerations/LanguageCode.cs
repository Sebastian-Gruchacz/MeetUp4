namespace MeetUp.Enumerations
{
    using System.ComponentModel;

    public enum LanguageCode
    {
        Unknown = 0,

        [Description(@"en-US")]
        English = 1,

        [Description(@"da-DK")]
        Danish = 2,

        [Description(@"sv-SE")]
        Swedish = 3
    }
}