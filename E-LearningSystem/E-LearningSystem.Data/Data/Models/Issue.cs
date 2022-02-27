namespace E_LearningSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;  
   
    using static DataConstants.Issue;

    public class Issue : BaseEntity
    {     
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(IssueTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(IssueDescriptionMaxLength)]
        public string Description { get; set; }

        DateTime? CreationDate { get; set; }
        
        DateTime? ResolvingDate { get; set; }

        public string UserId { get; set; }


        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}
