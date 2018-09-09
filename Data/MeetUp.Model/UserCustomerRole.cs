namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    public partial class UserCustomerRole : ITrackeable
    {
        [Key]
        public int Id { get; set; }
        
        public Guid AspNetUserId { get; set; }
        
        public int AspNetRoleId { get; set; }

        [ForeignKey(nameof(AspNetRoleId))]
        public virtual AspNetRole AspNetRole { get; set; }

        [ForeignKey(nameof(AspNetUserId))]
        public virtual AspNetUser AspNetUser { get; set; }

        public EntityTracker Tracking { get; set; }
    }
}