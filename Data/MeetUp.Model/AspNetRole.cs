namespace MeetUp.Model
{
    using System.ComponentModel.DataAnnotations;

    public partial class AspNetRole
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}