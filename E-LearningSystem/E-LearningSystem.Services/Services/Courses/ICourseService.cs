namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Services.Services.Courses.Models;
    
    public interface ICourseService
    {
        Task<int> CreateCourse(string userId, int trainerId, string name, string description, double price, int categoryId, string pictureFileName);

        Task<(bool, string)> EditCourse(int courseId, string name, string description, double price, int categoryId, string pictureFileName);

        Task<(bool, string)> DeleteCourse(int courseId);

        bool CheckIfCourseCategoryExists(int categoryId);

        Task<CourseServiceModel> GetCourseById(int id);

        Task<int> GetCourseCreatorId(int courseId);

        Task<CourseDetailsServiceModel> GetCourseDetails(int courseId);

        Task<IEnumerable<CourseCategoriesServiceModel>> GetAllCourseCategories();

        Task<IEnumerable<AllCoursesServiceModel>> GetAllCourses();

        Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(string userId);

        Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(int trainerId);

        Task<IEnumerable<LatestCoursesServiceModel>> GetLatestCourses(int count);

    }
}
