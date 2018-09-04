namespace MeetUp.Model
{
    public partial class SupplierOrderPolicy
    {
        public bool SendOrderViaEmail { get; set; }

        public string OrderEmailAddress { get; set; }

        public int MinEmployeeLimitForOrder { get; set; }
    }
}