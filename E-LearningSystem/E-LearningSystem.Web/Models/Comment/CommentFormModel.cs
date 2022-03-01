namespace E_LearningSystem.Web.Models.Comment
{
    using System.ComponentModel.DataAnnotations;

    public class CommentFormModel
    {
        [Required]
        public string Content { get; set; }
    }
}
