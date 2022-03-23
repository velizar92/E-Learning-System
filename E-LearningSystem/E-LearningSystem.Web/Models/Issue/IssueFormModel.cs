namespace E_LearningSystem.Web.Models.Issue
{
    using System.ComponentModel.DataAnnotations;

    using static E_LearningSystem.Data.DataConstants.Issue;
    using static E_LearningSystem.Infrastructure.Messages.ModelValidationMessages;

    public class IssueFormModel
    {
        [Required]
        public int CourseId { get; set; }

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(
            IssueTitleMaxLength,
            MinimumLength = IssueTitleMinLength,
            ErrorMessage = MinMaxLengthValidation)]
        public string Title { get; set; }


        [StringLength(
            IssueDescriptionMaxLength,
            MinimumLength = IssueDescriptionMinLength,
            ErrorMessage = MinMaxLengthValidation)]
        [Required(ErrorMessage = FieldRequired)]
        public string Description { get; set; }
    }
}
