namespace MeetUp.Enumerations
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Definitely should became enum.</remarks>
    public static class CaseStatus
    {
        public static string PendingSupplier = "Pending-supplier";
        public static string PendingCustomer = "Pending-customer";
        public static string PendingInternal = "Pending-Internal";
        public static string Closed = "Closed";
        public static string ClosedWithOutAgreement = "Closed-Without-Agreement";
    }
}