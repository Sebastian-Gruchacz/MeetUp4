namespace MeetUp.Common
{
    using System;

    public interface ITrackeable
    {
        DateTime? ModifiedDateTimeUtc { get; set; }

        DateTime CreatedDateTimeUtc { get; set; }

        Guid CreatedBy { get; set; }

        Guid? LastModifiedBy { get; set; }
    }
}