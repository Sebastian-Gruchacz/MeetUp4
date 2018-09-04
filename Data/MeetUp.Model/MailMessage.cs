﻿namespace MeetUp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    using Enumerations;

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class MailMessage
    {
        public MailMessage()
        {
            Attachments = new HashSet<MailAttachment>();
        }

        [Key]
        public Guid MessageId { get; set; }

        public string DisplayName { get; set; }
        public string Body { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Customer Customer { get; set; }

        public string FromAddress { get; set; }
        public int? CustomerId { get; set; }
        public int? DepartmentId { get; set; }
        public int LeadTrackingId { get; set; }
        public MessageKind Kind { get; set; }
        public MessageType Type { get; set; }
        public MessageStatus Status { get; set; }
        public string ToAddress { get; set; }
        public Guid? UserId { get; set; }
        public bool HideFromUser { get; set; }
        public string Subject { get; set; }

        public virtual ICollection<MailAttachment> Attachments { get; set; }
        
    }
}