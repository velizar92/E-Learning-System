namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Services.Services.Courses.Models;

    public interface ICourseService
    {
        Task<int> CreateCourse(string _name, string _description, string _imageUrl, int _categoryId);

        Task<bool> EditCourse(string _name, string _description, string _imageUrl, int _categoryId);

        Task<bool> DeleteCourse(int _courseId);

        Task<bool> CheckIfCourseCategoryExists(int _categoryId);

        Task<CourseDetailsServiceModel> GetCourseDetails(int _courseId);

        Task<IEnumerable<CourseCategoriesServiceModel>> GetAllCourseCategories();

        Task<IEnumerable<AllCoursesServiceModel>> GetAllCourses();

        Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(string _userId);

        Task<IEnumerable<LatestCoursesServiceModel>> GetLatestCourses();

    }
}
