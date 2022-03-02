namespace E_LearningSystem.Web.Models.Comment
{
    using System.ComponentModel.DataAnnotations;

    public class CommentFormModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
