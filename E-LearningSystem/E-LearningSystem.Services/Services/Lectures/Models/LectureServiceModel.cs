namespace E_LearningSystem.Services.Services.Lectures.Models
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Data.Models;
   
    public class LectureServiceModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public Resource[] Resources { get; set; }
    }
}
