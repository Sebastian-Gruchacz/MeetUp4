namespace MeetUp.Enumerations
{
    using System.ComponentModel;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Probably this should not be enum, this conversion to description is a bit hardcore. On the other hand - it works ;-)</remarks>
    public enum LanguageCode
    {
        Unknown = 0,

        [Description(@"en-US")]
        English = 1,

        [Description(@"da-DK")]
        Danish = 2,

        // NOTE: This language is not available at start, this is part of the exercise to enable it.

        //[Description(@"sv-SE")]
        //Swedish = 3
    }
}