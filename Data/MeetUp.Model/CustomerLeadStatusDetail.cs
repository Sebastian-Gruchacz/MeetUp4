namespace OrderService
{
    using System;

    public class CustomerLeadStatusDetail
    {
        public Guid CreatedUserId { get; set; }
        public string LeadStatus { get; set; }
        public string UserComments { get; set; }
    }
}