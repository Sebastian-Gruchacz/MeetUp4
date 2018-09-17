namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class OrderFormHistory
    {
        [Key]
        public Guid FormGuidId { get; set; }

        public Guid OrderId { get; set; }

        [DataType(@"ntext")]
        public string Json { get; set; }
    }
}