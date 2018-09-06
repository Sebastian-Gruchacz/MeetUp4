namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    public class CustomerLeadStatusDetail : ITrackeable
    {
        [Key]
        public int TransactionId { get; set; }

        public string LeadStatus { get; set; }

        public int LeadTrackingId { get; set; }

        public string UserComments { get; set; }

        /// <inheritdoc />
        [Required]
        public Guid CreatedBy { get; set; }

        /// <inheritdoc />
        [Required]
        public DateTime CreatedDateTimeUtc { get; set; }

        /// <inheritdoc />
        public Guid? LastModifiedBy { get; set; }

        /// <inheritdoc />
        public DateTime? ModifiedDateTimeUtc { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual AspNetUser Creator { get; set; }

        [ForeignKey(nameof(LastModifiedBy))]
        public virtual AspNetUser LastEditor { get; set; }

        [ForeignKey(nameof(LeadTrackingId))]
        public virtual CustomerLeadStatus ParentLead { get; set; }
    }
}