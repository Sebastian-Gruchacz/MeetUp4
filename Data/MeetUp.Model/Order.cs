namespace MeetUp.Model
{
    using System;
    using System.Collections.Generic;

    public partial class Order
    {
        public Order()
        {
            // ReSharper disable once VirtualMemberCallInConstructor (Creating default, empty collections for new entities)
            OrderLines = new HashSet<OrderLine>();
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public bool Deleted { get; set; }

        public DateTime Created { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}