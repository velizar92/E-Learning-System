namespace E_LearningSystem.Services.Services
{
    
    public class AllLectureCommentsServiceModel
    {
        
        public int Id { get; set; }
   
        public int LectureId { get; set; }
      
        public string Content { get; set; }
    
        public string UserId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }


    }
}