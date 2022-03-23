namespace E_LearningSystem.Web.Models.Course
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Services.Services.Courses.Models;

    using static E_LearningSystem.Data.DataConstants.Course;
    using static E_LearningSystem.Infrastructure.Messages.ModelValidationMessages;

    public class CourseFormModel
    {
      
        [Required(ErrorMessage = FieldRequired)]
        public string Name { get; set; }


        [Required(ErrorMessage = FieldRequired)]
        [StringLength(
            CourseDescriptionMaxLength,
            MinimumLength = CourseDescriptionMinLength,
            ErrorMessage = MinMaxLengthValidation)]
        public string Description { get; set; }


        [Required(ErrorMessage = FieldRequired)]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }


        [Required(ErrorMessage = FieldRequired)]
        [Display(Name = "Category")]
        public int CategoryId { get; init; }


        [Required(ErrorMessage = FieldRequired)]
        public IFormFile PictureFile { get; set; }


        public IEnumerable<CourseCategoriesServiceModel> Categories { get; set; }
    }
}
