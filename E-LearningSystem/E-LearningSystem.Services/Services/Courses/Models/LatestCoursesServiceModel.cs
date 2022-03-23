namespace E_LearningSystem.Services.Services.Courses.Models
{ 
    using E_LearningSystem.Data.Models;
    
    public class LatestCoursesServiceModel
    {   
        public int Id { get; set; }
       
        public string Name { get; set; }
  
        public string Description { get; set; }
  
        public double Price { get; set; }
     
        public string ImageUrl { get; set; }
    
        public string CategoryName { get; set; }
     
        public int? AssignedStudents { get; set; }
   
        public Trainer Trainer { get; set; }


    }
}
