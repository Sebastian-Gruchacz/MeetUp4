namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class SupplierCaseLimits : ITrackeable
    {
        [Key]
        public int Id { get; set; }

        public int SupplierId { get; set; }

        [StringLength(10)]
        public string CreditRating { get; set; }

        [StringLength(20)]
        public string PaymentMethod { get; set; }

        public int? MaxCaseLimit { get; set; }

        public bool LimitEnabled { get; set; }

        public virtual Supplier Supplier { get; set; }

        public EntityTracker Tracking { get; set; }
    }
}