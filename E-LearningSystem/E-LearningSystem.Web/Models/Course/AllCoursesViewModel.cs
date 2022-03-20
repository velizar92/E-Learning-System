namespace E_LearningSystem.Web.Models.Course
{
    using System.ComponentModel.DataAnnotations;

    public class AllCoursesViewModel
    {     
        
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public int? AssignedStudents { get; set; }

        [Required]
        public string ProfileImageUrl { get; set; }

        [Required]
        public string TrainerName { get; set; }


    }
}
