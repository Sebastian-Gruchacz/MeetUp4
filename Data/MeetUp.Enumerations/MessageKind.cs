namespace MeetUp.Enumerations
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Incoming and outgoing messages are very different animals, yet they are stored in the same table. Probably this is wrong, and should be fixed.</remarks>
    public enum MessageKind
    {
        Unknown = 0,
        Received,
        Sent,
        Archived
    }
}