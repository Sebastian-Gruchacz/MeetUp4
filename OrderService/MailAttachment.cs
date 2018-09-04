namespace OrderService
{
    using System;

    internal class MailAttachment
    {
        public Guid MessageId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FilePath { get; set; }
        public string MimeType { get; set; }
        public DateTime CreatedUtcDateTime { get; set; }
    }
}