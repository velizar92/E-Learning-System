namespace E_LearningSystem.Data.Models
{
    using E_LearningSystem.Data.Data.Enums;
    using E_LearningSystem.Data.Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class Trainer : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CVUrl { get; set; }

        [Required]
        public string FullName { get; set; }

        public TrainerStatus Status { get; set; }

        [Required]      
        public string UserId { get; set; }

    }
}
