namespace MeetUp.Enumerations
{
    /// <summary>
    /// Specifies whether user read the message - used by internal (in-0app) mail-viewer.
    /// </summary>
    public enum MessageStatus
    {
        Unknwon = 0, // TODO: spelling to be fixed! But carefully - will require UPDATE during migration!
        Unread,
        Read,
    }
}