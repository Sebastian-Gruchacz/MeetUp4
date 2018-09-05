namespace MeetUp.Model
{
    using System.ComponentModel.DataAnnotations;

    public partial class AspNetRole
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }
    }
}