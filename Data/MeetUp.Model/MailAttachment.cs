namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class MailAttachment : ITrackeable
    {
        public Guid MessageId { get; set; }

        [StringLength(250)]
        public string FileName { get; set; }

        [StringLength(250)]
        public string FileUrl { get; set; }

        [StringLength(250)]
        public string FilePath { get; set; }

        [StringLength(50)]
        public string MimeType { get; set; }

        public DateTime CreatedUtcDateTime { get; set; }

        /// <inheritdoc />
        public DateTime? ModifiedDateTimeUtc { get; set; }

        /// <inheritdoc />
        public DateTime CreatedDateTimeUtc { get; set; }

        /// <inheritdoc />
        public Guid CreatedBy { get; set; }

        /// <inheritdoc />
        public Guid? LastModifiedBy { get; set; }
    }
}