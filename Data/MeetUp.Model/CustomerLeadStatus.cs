namespace OrderService
{
    using System;
    using System.Collections.Generic;

    public partial class CustomerLeadStatus
    {
        public CustomerLeadStatus()
        {
            CustomerLeadStatusDetail = new HashSet<CustomerLeadStatusDetail>();
        }

        public virtual ICollection<CustomerLeadStatusDetail> CustomerLeadStatusDetail { get; set; }
        public int ForSupplierId { get; set; }
        public int FromCustomerId { get; set; }
        public int FromDepartmentId { get; set; }
        public Guid FromUserId { get; set; }
        public Guid? OrderId { get; set; }
    }
}