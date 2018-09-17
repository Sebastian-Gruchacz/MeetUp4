namespace MeetUp.Model
{
    using System.ComponentModel.DataAnnotations;

    public partial class OrderLine
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        [StringLength(20)]
        public string LineKey { get; set; }

        [StringLength(100)]
        public string LineValue { get; set; }
    }
}