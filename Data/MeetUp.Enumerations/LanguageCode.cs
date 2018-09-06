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

        // NOTE: This language is not available at start, this is part of the exercise to enable it

        //[Description(@"sv-SE")]
        //Swedish = 3
    }
}