namespace EmailingService
{
    using MeetUp.BLL;
    using MeetUp.Model;

    public class MailAttachmentService : IMailAttachmentService
    {
        private readonly IMailAttachmentRepository _mailAttachmentRepository;

        public MailAttachmentService(IMailAttachmentRepository mailAttachmentRepository)
        {
            _mailAttachmentRepository = mailAttachmentRepository;
        }

        public void SaveAttachemnts(MailAttachment attachment)
        {
            _mailAttachmentRepository.Insert(attachment);
            _mailAttachmentRepository.SaveChanges(); // This is bad, really bad, need unit-of-work
        }
    }
}