namespace E_LearningSystem.Web.Models.Home
{
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Services.Services.Courses.Models;

    public class IndexViewModel
    {      
        public int CoursesCount { get; set; }
        public int TrainersCount { get; set; }
        public int LearnersCount { get; set; }
        public IEnumerable<LatestCoursesServiceModel> LatestCourses { get; set; }
        public IEnumerable<AllTrainersServiceModel> TopTrainers { get; set; }
    }
}
