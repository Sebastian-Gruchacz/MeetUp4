namespace MeetUp.Model
{
    using System.ComponentModel.DataAnnotations;

    public partial class OrderLine
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
    }
}