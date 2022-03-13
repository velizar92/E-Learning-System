namespace E_LearningSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Testimonial : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }


    }
}
