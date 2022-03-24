namespace E_LearningSystem.Web.Models.Lecture
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Data.Models;

    using static E_LearningSystem.Data.DataConstants.Lecture;
    using static E_LearningSystem.Infrastructure.Messages.ModelValidationMessages;

    public class CreateLectureFormModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }


        [Required(ErrorMessage = FieldRequired)]
        [StringLength(
            LectureNameMaxLength,
            MinimumLength = LectureNameMinLength,
            ErrorMessage = MinMaxLengthValidation)]
        public string Name { get; set; }


        [Required(ErrorMessage = FieldRequired)]
        [StringLength(
            LectureDescriptionMaxLength,
            MinimumLength = LectureDescriptionMinLength,
            ErrorMessage = MinMaxLengthValidation)]
        public string Description { get; set; }

        public Resource[]? Resources { get; set; }

        public IEnumerable<IFormFile> Files { get; set; }
    }
}
