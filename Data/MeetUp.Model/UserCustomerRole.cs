namespace MeetUp.Model
{
    using System;

    public partial class UserCustomerRole
    {
        public Guid AspNetUserId { get; set; }

        public int AspNetRoleId { get; set; }


        public virtual AspNetRole AspNetRole { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}