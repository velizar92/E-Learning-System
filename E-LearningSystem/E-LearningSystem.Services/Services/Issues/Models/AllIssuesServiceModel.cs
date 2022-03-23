namespace E_LearningSystem.Services.Services
{  

    public class AllIssuesServiceModel
    {    
        public int IssueId { get; set; }
       
        public int CourseId { get; set; }
   
        public string CourseName { get; set; }
       
        public string Title { get; set; }
   
        public string Description { get; set; }
    }
}