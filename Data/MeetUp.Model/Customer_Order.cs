namespace MeetUp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    using MeetUp.Enumerations;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>NOTE: This name should really be refactored into simply "CustomerOrder", but that would conflict names with original service name. Left as training example.</remarks>
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class Customer_Order
    {
        public Customer_Order()
        {
            OrderLines = new HashSet<OrderLine>();
            CustomerOrderAttachments = new HashSet<CustomerOrderAttachment>();
        }

        [Key]
        public Guid Id { get; set; }

        public int ForSupplierId { get; set; }

        public int FromCustomerId { get; set; }

        public int FromDepartmentId { get; set; }

        public bool Deleted { get; set; }

        [StringLength(150)]
        public string OrderEmail { get; set; }

        public Guid CreatedByUserId { get; set; }

        [StringLength(10)]
        [Required]
        [Column("Status")]
        public string StatusString
        {
            get => Status.ToString();
            set => Status = value.ParseEnum<OrderStatus>();
        }

        [NotMapped]
        public OrderStatus Status { get; set; }

        public bool? IsJson { get; set; }

        [DataType(@"ntext")]
        public string OrderXML;


        public DateTime SentDateTimeUtc;

        public DateTime DateCreatedUtc { get; set; }


        public virtual ICollection<OrderLine> OrderLines { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<CustomerOrderAttachment> CustomerOrderAttachments { get; set; }
    }
}