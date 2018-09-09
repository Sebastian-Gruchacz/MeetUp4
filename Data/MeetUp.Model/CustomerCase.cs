namespace MeetUp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class CustomerCase : ITrackeable
    {
        public CustomerCase()
        {
            this.CaseHistory = new HashSet<CustomerCaseStatusEntry>();
        }

        [Key]
        public int CaseId { get; set; }

        public int ForSupplierId { get; set; }

        public int FromCustomerId { get; set; }

        public int FromDepartmentId { get; set; }
        
        /// <summary>
        /// Nullable -> CustomerCase can be created for the whole Customer or requested by Call-Center employee
        /// </summary>
        public Guid? FromUserId { get; set; }

        /// <summary>Order is optional - this is just one type of the Customer Case.</summary>
        public Guid? OrderId { get; set; }

        [InverseProperty(nameof(CustomerCaseStatusEntry.ParentCase))]
        public virtual ICollection<CustomerCaseStatusEntry> CaseHistory { get; set; }

        /// <inheritdoc />
        public EntityTracker Tracking { get; set; }
    }
}