namespace E_LearningSystem.Services.Services
{
    using System.ComponentModel.DataAnnotations;

    public class AllLectureCommentsServiceModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int LectureId { get; set; }

        [Required]
        public string Content { get; set; }
      
    }
}