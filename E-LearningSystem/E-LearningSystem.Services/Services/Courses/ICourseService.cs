namespace E_LearningSystem.Services.Services
{
    using Microsoft.AspNetCore.Http;
    using E_LearningSystem.Services.Services.Courses.Models;
    
    public interface ICourseService
    {
        Task<int> CreateCourse(string userId, string name, string description, int categoryId, IFormFile pictureFile);

        Task<bool> EditCourse(int courseId, string name, string description, int categoryId, IFormFile pictureFile);

        Task<bool> DeleteCourse(int courseId);

        bool CheckIfCourseCategoryExists(int categoryId);

        Task<CourseServiceModel> GetCourseById(int id);

        Task<CourseDetailsServiceModel> GetCourseDetails(int courseId);

        Task<IEnumerable<CourseCategoriesServiceModel>> GetAllCourseCategories();

        Task<IEnumerable<AllCoursesServiceModel>> GetAllCourses();

        Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(string userId);
     
        Task<IEnumerable<LatestCoursesServiceModel>> GetLatestCourses(int count);

    }
}
