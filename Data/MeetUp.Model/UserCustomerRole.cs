namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public partial class UserCustomerRole : ITrackeable
    {
        [Key]
        public int Id { get; set; }

        public Guid AspNetUserId { get; set; }

        public int AspNetRoleId { get; set; }

        public virtual AspNetRole AspNetRole { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

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