namespace MeetUp.Model
{
    public partial class CustomerOrderAttachment
    {
        public string FileURL;

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string MimeType { get; set; }
    }
}