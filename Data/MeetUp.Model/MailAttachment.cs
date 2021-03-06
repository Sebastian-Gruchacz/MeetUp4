﻿namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    public class MailAttachment : ITrackeable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid MessageId { get; set; }

        [StringLength(250)]
        public string FileName { get; set; }

        [StringLength(250)]
        public string FileUrl { get; set; }

        [StringLength(250)]
        public string FilePath { get; set; }

        [StringLength(50)]
        public string MimeType { get; set; }

        [ForeignKey(nameof(MessageId))]
        public virtual MailMessage MailMessage { get; set; }

        /// <inheritdoc />
        public EntityTracker Tracking { get; set; }
    }
}