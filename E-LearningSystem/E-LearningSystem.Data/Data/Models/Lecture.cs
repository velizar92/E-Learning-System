namespace E_LearningSystem.Data.Models
{
    using E_LearningSystem.Data.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.Lecture;

    public class Lecture : BaseEntity
    {
        public Lecture()
        {
            this.Resources = new HashSet<Resource>();
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(LectureNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(LectureDescriptionMaxLength)]
        public string Description { get; set; }


        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; }


        public ICollection<Resource> Resources { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
