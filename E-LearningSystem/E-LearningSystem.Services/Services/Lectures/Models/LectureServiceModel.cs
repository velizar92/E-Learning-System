namespace E_LearningSystem.Services.Services.Lectures.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LectureServiceModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
