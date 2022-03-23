namespace E_LearningSystem.Services.Services.Courses.Models
{

    public class AllCoursesServiceModel
    {       
        public int Id { get; set; }
   
        public int TrainerId { get; set; }
        
        public string Name { get; set; }
      
        public string Description { get; set; }
     
        public double Price { get; set; }
    
        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }
     
        public int? AssignedStudents { get; set; }
    }
}
