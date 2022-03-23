namespace E_LearningSystem.Web.Models.Course
{  
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Lectures.Models;
    
    public class CourseDetailsViewModel
    {      
        public int Id { get; set; }

       
        public string Name { get; set; }

      
        public string Description { get; set; }

       
        public string ImageUrl { get; set; }

       
        public int? AssignedStudents { get; set; }

       
        public double Price { get; set; }

        
        public Trainer Trainer { get; set; }

        public bool HasCourse { get; set; }

        public IEnumerable<LectureServiceModel> Lectures { get; set; }

    }
}
