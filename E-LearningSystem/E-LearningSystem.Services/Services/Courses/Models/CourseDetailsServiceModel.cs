namespace E_LearningSystem.Services.Services.Courses.Models
{  
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Lectures.Models;
   
    public class CourseDetailsServiceModel
    {  
        public int Id { get; set; }
    
        public string Name { get; set; }
   
        public string Description { get; set; }
     
        public string ImageUrl { get; set; }
    
        public int? AssignedStudents { get; set; }
    
        public double Price { get; set; }
    
        public Trainer Trainer { get; set; }

        public IEnumerable<LectureServiceModel> Lectures { get; set; }
    }
}