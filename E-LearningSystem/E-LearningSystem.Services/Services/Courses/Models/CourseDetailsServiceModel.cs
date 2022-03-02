namespace E_LearningSystem.Services.Services.Courses.Models
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Services.Services.Lectures.Models;
   
    public class CourseDetailsServiceModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public IEnumerable<LectureServiceModel> Lectures { get; set; }
    }
}