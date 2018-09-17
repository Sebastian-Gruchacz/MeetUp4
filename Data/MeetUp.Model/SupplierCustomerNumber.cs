namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// External number of Customer, given by corresponding supplier
    /// </summary>
    public partial class SupplierCustomerNumber
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int DepartmentId { get; set; }

        public int SupplierId { get; set; }

        [StringLength(200)]
        public string SupplierCustomerId { get; set; }

        public DateTime? CreatedUtcDateTime { get; set; }

        public DateTime? LastUpdatedUtcDateTime { get; set; }

        public bool ExportedToErp { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Department Department { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}