namespace E_LearningSystem.Web.Models.Course
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Services.Services.Courses.Models;
   
    public class CourseFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<CourseCategoriesServiceModel> Categories { get; set; }
    }
}
