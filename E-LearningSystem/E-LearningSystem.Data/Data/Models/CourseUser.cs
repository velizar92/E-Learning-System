using E_LearningSystem.Data.Models;

namespace E_LearningSystem.Data.Data.Models
{
    public class CourseUser
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
