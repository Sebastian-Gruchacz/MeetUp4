namespace OrderService
{
    using System;

    public class CustomerLeadStatusDetail
    {
        public DateTime CreatedUTCDateTime;

        public DateTime LastUpdatedUTCDateTime;

        public Guid CreatedUserId { get; set; }

        public string LeadStatus { get; set; }

        public string UserComments { get; set; }

        public int TransactionId { get; set; }
    }
}