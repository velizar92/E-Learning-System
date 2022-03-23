namespace E_LearningSystem.Web.Models.Comment
{
    using System.ComponentModel.DataAnnotations;  
    using static E_LearningSystem.Data.DataConstants.User;
    using static E_LearningSystem.Data.DataConstants.Comment;
    using static E_LearningSystem.Infrastructure.Messages.ModelValidationMessages;

    public class CommentFormModel
    {
        [Required(ErrorMessage = FieldRequired)]
        [StringLength(
            CommentContentMaxLength,
            MinimumLength = CommentContentMinLength,
            ErrorMessage = MinMaxLengthValidation)]
        public string Content { get; set; }


        [Required(ErrorMessage = FieldRequired)]
        [StringLength(
            FirstNameMaxLength,
            MinimumLength = FirstNameMinLength,
            ErrorMessage = MinMaxLengthValidation)]
        public string FirstName { get; set; }


        [StringLength(
           LastNameMaxLength,
           MinimumLength = LastNameMinLength,
           ErrorMessage = MinMaxLengthValidation)]
        [Required(ErrorMessage = FieldRequired)]
        public string LastName { get; set; }


        [Required(ErrorMessage = FieldRequired)]
        public string ProfileImageUrl { get; set; }


        [Required(ErrorMessage = FieldRequired)]
        public int LectureId { get; set; }
    }
}
