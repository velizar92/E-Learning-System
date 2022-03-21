namespace E_LearningSystem.Services.Services
{
    using System.ComponentModel.DataAnnotations;

    public class AllTrainersServiceModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string ProfileImageUrl { get; set; }

        [Required]
        public int? Rating { get; set; }
    }
}