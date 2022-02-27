namespace E_LearningSystem.Services.Services
{
    using Microsoft.AspNetCore.Http;
    using E_LearningSystem.Services.Services.Courses.Models;
    
    public interface ICourseService
    {
        Task<int> CreateCourse(string _userId, string _name, string _description, int _categoryId, IFormFile _pictureFile);

        Task<bool> EditCourse(int _courseId, string _name, string _description, string _imageUrl, int _categoryId);

        Task<bool> DeleteCourse(int _courseId);

        bool CheckIfCourseCategoryExists(int _categoryId);

        Task<CourseDetailsServiceModel> GetCourseDetails(int _courseId);

        Task<IEnumerable<CourseCategoriesServiceModel>> GetAllCourseCategories();

        Task<IEnumerable<AllCoursesServiceModel>> GetAllCourses();

        Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(string _userId);

        Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(int _trainerId);

        Task<IEnumerable<LatestCoursesServiceModel>> GetLatestCourses(int _count);

    }
}
