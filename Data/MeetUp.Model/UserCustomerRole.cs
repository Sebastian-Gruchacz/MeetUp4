namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    public partial class UserCustomerRole : ITrackeable
    {
        [Key]
        public int Id { get; set; }
        
        public Guid AspNetUserId { get; set; }
        
        public int AspNetRoleId { get; set; }

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

        [ForeignKey(nameof(AspNetRoleId))]
        public virtual AspNetRole AspNetRole { get; set; }

        [ForeignKey(nameof(AspNetUserId))]
        public virtual AspNetUser AspNetUser { get; set; }
    }
}