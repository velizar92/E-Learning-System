namespace E_LearningSystem.Services.Services.Courses.Models
{
    public class CourseServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public int AssignedStudents { get; set; }
    }
}
