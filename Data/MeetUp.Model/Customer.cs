namespace MeetUp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    using Common;

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class Customer : ITrackeable
    {
        public Customer()
        {
            UserRolesInCustomers = new HashSet<UserCustomerRole>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        [Required]
        [Index("UIX_CustomerName", 1, IsUnique = true)]
        public string CustomerName { get; set; }

        [Required]
        public int ChannelId { get; set; }

        [Required]
        public int NoOfEmployee { get; set; }

        /// <summary>
        /// This is internal email that is used to extract direct emails from catch-all in-box.
        /// </summary>
        [StringLength(250)]
        [Index("UIX_InternaEmail", 1, IsUnique = true)]
        public string InternalEmail { get; set; }

        public virtual ICollection<UserCustomerRole> UserRolesInCustomers { get; set; }

        /// <inheritdoc />
        public EntityTracker Tracking { get; set; }
    }
}