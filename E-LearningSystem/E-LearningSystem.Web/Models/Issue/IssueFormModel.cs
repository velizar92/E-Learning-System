namespace E_LearningSystem.Web.Models.Issue
{
    using System.ComponentModel.DataAnnotations;

    public class IssueFormModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
