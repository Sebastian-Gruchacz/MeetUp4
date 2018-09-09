namespace MeetUp.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class stripped from any meaningful data, not really needed in example.
    /// </summary>
    public partial class AspNetRole
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }
    }
}