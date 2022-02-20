namespace E_LearningSystem.Data.Models
{
    using E_LearningSystem.Data.Data.Enums;
    using System.ComponentModel.DataAnnotations;

    public class Trainer
    {
        public Trainer()
        {
            this.Courses = new HashSet<Course>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string CVUrl { get; set; }

        public TrainerStatus Status { get; set; }

        [Required]      
        public string UserId { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
