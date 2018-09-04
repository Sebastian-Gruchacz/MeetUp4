namespace MeetUp.Model
{
    using System.ComponentModel.DataAnnotations;

    public partial class CustomerOrderAttachment
    {
        [Key]
        public int Id { get; set; }
    
        [StringLength(250)]
        public string FileURL;

        [StringLength(250)]
        public string FileName { get; set; }

        [StringLength(250)]
        public string FilePath { get; set; }

        [StringLength(50)]
        public string MimeType { get; set; }
    }
}