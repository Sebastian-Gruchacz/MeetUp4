namespace MeetUp.Model
{
    using System;

    using Common;

    public class CustomerLeadStatusDetail : ITrackeable
    {
        public DateTime CreatedUTCDateTime;

        public DateTime LastUpdatedUTCDateTime;

        public Guid CreatedUserId { get; set; }

        public string LeadStatus { get; set; }

        public string UserComments { get; set; }

        public int TransactionId { get; set; }

        /// <inheritdoc />
        public DateTime? ModifiedDateTimeUtc { get; set; }

        /// <inheritdoc />
        public DateTime CreatedDateTimeUtc { get; set; }

        /// <inheritdoc />
        public Guid CreatedBy { get; set; }

        /// <inheritdoc />
        public Guid? LastModifiedBy { get; set; }
    }
}