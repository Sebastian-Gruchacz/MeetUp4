namespace MeetUp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class CustomerLeadStatus : ITrackeable
    {
        public CustomerLeadStatus()
        {
            this.CustomerLeadStatusDetails = new HashSet<CustomerLeadStatusDetail>();
        }

        [Key]
        public int LeadTrackingId { get; set; }

        public int ForSupplierId { get; set; }

        public int FromCustomerId { get; set; }

        public int FromDepartmentId { get; set; }
        
        /// <summary>
        /// NUllable -> Lead can be created for the whole Customer or requested by Call-Center employee
        /// </summary>
        public Guid? FromUserId { get; set; }

        /// <summary>Order is optional - this is just one type of the Lead.</summary>
        public Guid? OrderId { get; set; }

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

        [InverseProperty(nameof(CustomerLeadStatusDetail.ParentLead))]
        public virtual ICollection<CustomerLeadStatusDetail> CustomerLeadStatusDetails { get; set; }
    }
}