namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [ComplexType]
    public partial class EntityTracker
    {
        public Guid CreatedBy { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public DateTime? ModifiedDateTimeUtc { get; set; }

        // Maybe weak relations for tracking be better?

        //[ForeignKey(nameof(CreatedBy))]
        //public virtual AspNetUser Creator { get; set; }

        //[ForeignKey(nameof(LastModifiedBy))]
        //public virtual AspNetUser LastEditor { get; set; }

        public static EntityTracker StartTracking(Guid creatorId, DateTime? createDateTime = null)
        {
            return new EntityTracker
            {
                CreatedBy = creatorId,
                CreatedDateTimeUtc = createDateTime ?? DateTime.UtcNow
            };
        }

        public static EntityTracker UpdateTracking(EntityTracker currentTrackingRecord, Guid editorId, DateTime? modifiedDateTime = null)
        {
            if (currentTrackingRecord == null)
            {
                throw new ArgumentNullException(nameof(currentTrackingRecord));
            }

            return new EntityTracker
            {
                CreatedBy = currentTrackingRecord.CreatedBy,
                CreatedDateTimeUtc = currentTrackingRecord.CreatedDateTimeUtc,
                LastModifiedBy = editorId,
                ModifiedDateTimeUtc = modifiedDateTime ?? DateTime.UtcNow
            };
        }
    }
}
