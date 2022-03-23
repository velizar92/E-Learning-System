namespace E_LearningSystem.Services.Services.Lectures.Models
{
    using E_LearningSystem.Data.Models;
   
    public class LectureServiceModel
    {      
        public int Id { get; set; }
      
        public int CourseId { get; set; }
     
        public string Name { get; set; }
   
        public string Description { get; set; }

        public Resource[] Resources { get; set; }
    }
}
