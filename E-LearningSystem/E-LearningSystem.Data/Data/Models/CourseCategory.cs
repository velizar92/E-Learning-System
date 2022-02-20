namespace E_LearningSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.CourseCategory;

    public class CourseCategory
    {
        public CourseCategory()
        {
            this.Courses = new HashSet<Course>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CourseCategoryNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
