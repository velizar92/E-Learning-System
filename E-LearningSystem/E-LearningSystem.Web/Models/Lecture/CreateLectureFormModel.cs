namespace E_LearningSystem.Web.Models.Lecture
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Data.Models;
    
    public class CreateLectureFormModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public Resource[] Resources { get; set; }
    }
}
