namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class AspNetUser
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Used as login and address to send emails to
        /// </summary>
        [StringLength(250)]
        [Index("UIX_UserLoginEmail", 1, IsUnique = true)]
        public string Email { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string LanguageCode { get; set; } // TODO: use enum

        /// <summary>
        /// This is internal email that is used to extract direct emails from catch-all in-box.
        /// </summary>
        [StringLength(250)]
        [Index("UIX_InternaEmail", 1, IsUnique = true)]
        public string InternalEmail { get; set; }
    }
}