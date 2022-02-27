namespace E_LearningSystem.Services.Services
{
    using System.ComponentModel.DataAnnotations;

    public class AllTrainersServiceModel
    {
      
        [Required]
        public string FullName { get; set; }

        [Required]
        public string ProfileImageUrl { get; set; }
    }
}