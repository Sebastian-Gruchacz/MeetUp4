namespace MeetUp.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Department is part of customer with separate address and order flow, yet shared payments flow (Not implemented in example)
    /// </summary>
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class Department : ITrackeable
    {
        public Department()
        {
            SupplierCustomerNumbers = new HashSet<SupplierCustomerNumber>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string DepartmentNumber { get; set; }

        /// <summary>
        /// This is internal email that is used to extract direct emails from catch-all in-box.
        /// </summary>
        [StringLength(250)]
        public string InternalEmail { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<SupplierCustomerNumber> SupplierCustomerNumbers { get; set; }

        /// <inheritdoc />
        public EntityTracker Tracking { get; set; }
    }
}