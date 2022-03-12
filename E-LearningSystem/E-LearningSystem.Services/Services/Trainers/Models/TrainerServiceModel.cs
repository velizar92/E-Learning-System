using System.ComponentModel.DataAnnotations;

namespace E_LearningSystem.Services.Services.Trainers.Models
{
    public class TrainerServiceModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string ProfileImage { get; set; }
    }
}
