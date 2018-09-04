namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class UserCustomerRole
    {
        [Key]
        public int Id { get; set; }

        public Guid AspNetUserId { get; set; }

        public int AspNetRoleId { get; set; }


        public virtual AspNetRole AspNetRole { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}