namespace MeetUp.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class CaseField : ITrackeable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CaseField()
        {
            SupplierCaseFields = new HashSet<SupplierCaseField>();
        }

        [Key]
        public int FieldId { get; set; }

        [Required]
        [StringLength(150)]
        public string FieldLabel { get; set; }

        [Required]
        [StringLength(150)]
        public string FieldValue { get; set; }

        [StringLength(150)]
        public string FieldDescription { get; set; }

        public bool IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierCaseField> SupplierCaseFields { get; set; }

        public EntityTracker Tracking { get; set; }
    }
}