namespace MeetUp.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class Customer
    {
        public Customer()
        {
            UserRolesInCustomers = new HashSet<UserCustomerRole>();
        }

        [Key]
        public int Id { get; set; }

        public int NoOfEmployee { get; set; }

        [StringLength(150)]
        [Required]
        [Index("UIX_CustomerName", 1, IsUnique = true)]
        public string CustomerName { get; set; }

        public int ChannelId { get; set; }

        public virtual ICollection<UserCustomerRole> UserRolesInCustomers { get; set; }
    }
}