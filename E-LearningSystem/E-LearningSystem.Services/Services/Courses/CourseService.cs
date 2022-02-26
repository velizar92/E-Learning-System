namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Services.Services.Courses.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CourseService : ICourseService
    {
        private readonly ELearningSystemDbContext dbContext;

        public CourseService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public Task<bool> CheckIfCourseCategoryExists(int _categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateCourse(string _name, string _description, string _imageUrl, int _categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCourse(int _courseId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditCourse(string _name, string _description, string _imageUrl, int _categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CourseCategoriesServiceModel>> GetAllCourseCategories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AllCoursesServiceModel>> GetAllCourses()
        {
            throw new NotImplementedException();
        }

        public Task<CourseDetailsServiceModel> GetCourseDetails(int _courseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LatestCoursesServiceModel>> GetLatestCourses()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(string _userId)
        {
            throw new NotImplementedException();
        }
    }
}
