namespace E_LearningSystem.Data.Models
{
    using E_LearningSystem.Data.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(Lecture))]
        public int LectureId { get; set; }
        public Lecture Lecture { get; set; }
     
    }
}
