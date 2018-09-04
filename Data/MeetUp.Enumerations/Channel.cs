namespace MeetUp.Enumerations
{
    public enum Channels
    {
        /// <summary>
        /// Not defined in original solution, fixing.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// This is default communication channel in company
        /// </summary>
        ChannelOne = 1,

        /// <summary>
        /// This is alternate channel for some legacy customers, that require different rules, messages and UI; Yet it shares some external services with ChannelOne.
        /// </summary>
        ChannelTwo = 12,

        /// <summary>
        /// Client is about to open new market in country with different rules and some crucial external services not available, but basically resembling ChannelOne
        /// </summary>
        ChannelNew = 2
    }
}
