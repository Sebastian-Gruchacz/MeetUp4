namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    public class CustomerCaseStatusEntry : ITrackeable
    {
        [Key]
        public int EntryId { get; set; }

        public string Status { get; set; } // TODO: enum

        public int CaseId { get; set; }

        public string UserComments { get; set; }

        [ForeignKey(nameof(CaseId))]
        public virtual CustomerCase ParentCase { get; set; }

        /// <inheritdoc />
        public EntityTracker Tracking { get; set; }
    }
}