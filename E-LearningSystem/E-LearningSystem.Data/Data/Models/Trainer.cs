namespace E_LearningSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Data.Data.Models;
    using E_LearningSystem.Data.Enums;
   
    public class Trainer : BaseEntity
    {
        public Trainer()
        {
            this.Courses = new HashSet<Course>();
            this.Votes = new HashSet<Vote>();
        }


        [Key]
        public int Id { get; set; }

        [Required]
        public string CVUrl { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string ProfileImageUrl { get; set; }

        public int? Rating { get; set; }

        public TrainerStatus Status { get; set; }

        [Required]      
        public string UserId { get; set; }

        public ICollection<Course> Courses { get; set; }

        public ICollection<Vote> Votes { get; set; }

    }
}
