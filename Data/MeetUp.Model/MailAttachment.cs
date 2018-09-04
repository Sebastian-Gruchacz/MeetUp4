namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MailAttachment
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
    }
}