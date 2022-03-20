namespace E_LearningSystem.Services.Services
{
    using System.ComponentModel.DataAnnotations;

    public class AllIssuesServiceModel
    {
        [Required]
        public int IssueId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}