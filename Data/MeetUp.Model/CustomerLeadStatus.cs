namespace MeetUp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OrderService;

    public partial class CustomerLeadStatus
    {


        public CustomerLeadStatus()
        {
            CustomerLeadStatusDetail = new HashSet<CustomerLeadStatusDetail>();
        }

        [Key]
        public int LeadTrackingId { get; set; }

        public int ForSupplierId { get; set; }
        public int FromCustomerId { get; set; }
        public int FromDepartmentId { get; set; }
        public Guid FromUserId { get; set; }
        public Guid? OrderId { get; set; }

        public DateTime? LastUpdatedUTCDateTime;
        public DateTime CreatedUTCDateTime;

        public virtual ICollection<CustomerLeadStatusDetail> CustomerLeadStatusDetail { get; set; }
    }
}