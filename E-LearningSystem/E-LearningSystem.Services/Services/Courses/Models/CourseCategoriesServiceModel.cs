namespace E_LearningSystem.Services.Services.Courses.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CourseCategoriesServiceModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
