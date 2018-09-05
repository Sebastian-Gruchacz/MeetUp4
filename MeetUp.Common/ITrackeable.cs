namespace MeetUp.Common
{
    using System;

    public interface ITrackeable
    {
        Guid CreatedBy { get; set; }

        DateTime CreatedDateTimeUtc { get; set; }
        
        Guid? LastModifiedBy { get; set; }

        DateTime? ModifiedDateTimeUtc { get; set; }
    }
}