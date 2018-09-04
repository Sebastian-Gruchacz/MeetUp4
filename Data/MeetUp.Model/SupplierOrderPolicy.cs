namespace MeetUp.Model
{
    using System.ComponentModel.DataAnnotations;

    public partial class SupplierOrderPolicy
    {
        [Key]
        public int Id { get; set; }

        public int SupplierId { get; set; }


        public bool SendOrderViaEmail { get; set; }

        /// <summary>
        /// Default address to send orders to
        /// </summary>
        [StringLength(250)]
        public string OrderEmailAddress { get; set; }

        public int MinEmployeeLimitForOrder { get; set; }
    }
}