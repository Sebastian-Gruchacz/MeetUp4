namespace EmailingService
{
    using System;

    public class MailMessageResponse
    {
        public Guid MessageId { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}