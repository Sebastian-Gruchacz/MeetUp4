﻿namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Enumerations;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>NOTE: This entity really shall be named "User", yet I left this as track of creation process - clearly this entity came from Asp.Net template and remained names so. Well...</remarks>
    public partial class AspNetUser : ITrackeable
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Used as login and address to send emails to
        /// </summary>
        [StringLength(250)]
        [Required]
        [Index("UIX_UserLoginEmail", 1, IsUnique = true)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(5)]
        [Required]
        [Column("LanguageCode")]
        public string LanguageCodeString
        {
            get => LanguageCode.GetLanguageCodeString();
            set => LanguageCode = LanguageExtensions.GetCode(value);
        }

        [NotMapped]
        public LanguageCode LanguageCode { get; set; }

        /// <summary>
        /// Represent service-type-user, user that one cannot log in to the system, but can alter entities
        /// </summary>
        public bool IsSystemUser { get; set; }

        /// <summary>
        /// This is internal email that is used to extract direct emails from catch-all in-box.
        /// </summary>
        [StringLength(250)]
        [Index("UIX_InternaEmail", 1, IsUnique = true)]
        public string InternalEmail { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }


        /// <inheritdoc />
        public EntityTracker Tracking { get; set; }
    }
}