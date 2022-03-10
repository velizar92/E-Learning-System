namespace E_LearningSystem.Services.Services.Resources.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AllResourcesServiceModel
    {
        [Required]
        public string ResourceName { get; set; }

        [Required]
        public string LectureName { get; set; }

    }
}
