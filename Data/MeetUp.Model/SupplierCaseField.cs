namespace MeetUp.Model
{
    using System.ComponentModel.DataAnnotations;

    public partial class SupplierCaseField : ITrackeable
    {
        [Key]
        public int Id { get; set; }

        public int CaseFieldId { get; set; }

        public int SupplierId { get; set; }

        public int ChannelId { get; set; }

        public int FieldOrder { get; set; }

        public bool IsActive { get; set; }

        [StringLength(100)]
        public string CaseValue { get; set; }

        [StringLength(100)]
        public string CustomLabel { get; set; }

        //public virtual Channel Channel { get; set; }

        public virtual CaseField CaseField { get; set; }

        public virtual Supplier Supplier { get; set; }

        public EntityTracker Tracking { get; set; }
    }
}