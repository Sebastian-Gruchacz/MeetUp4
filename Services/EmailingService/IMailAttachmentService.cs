﻿namespace EmailingService
{
    using MeetUp.Model;

    public interface IMailAttachmentService
    {
        void SaveAttachemnts(MailAttachment attachment);
    }
}