namespace MeetUp.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class Supplier
    {
        public Supplier()
        {
            SupplierOrderPolicies = new HashSet<SupplierOrderPolicy>();
        }

        [Key]
        public int SupplierId { get; set; }

        [StringLength(100)]
        [Required]
        [Index("UIX_SupplierName", 1, IsUnique = true)]
        public string Name { get; set; }



        /// <summary>
        /// This is internal address that handles special case orders for this supplier.
        /// </summary>
        /// <remarks>If NULL - default support address shall be used. Yet this is not properly implemented in example code. Exercise fixing it!</remarks>
        [StringLength(250)]
        [Index("UIX_InternaEmail", 1, IsUnique = true)]
        public string InternalSupportEmailForSpecialSituations { get; set; }




        public virtual ICollection<SupplierOrderPolicy> SupplierOrderPolicies { get; set; }

        public virtual ICollection<SupplierCaseField> SupplierCaseFields { get; set; }
    }
}