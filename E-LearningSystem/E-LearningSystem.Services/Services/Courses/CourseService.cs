namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Courses.Models;
   
    public class CourseService : ICourseService
    {
        private readonly ELearningSystemDbContext dbContext;

        public CourseService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public bool CheckIfCourseCategoryExists(int _categoryId)
        {
            if(dbContext.CourseCategories.Any(c => c.Id == _categoryId) == false)
            {
                return false;
            }

            return true;
        }


        public async Task<int> CreateCourse(string _name, string _description, string _imageUrl, int _categoryId)
        {
            Course course = new Course()
            {
                Name = _name,
                Description = _description,
                ImageUrl = _imageUrl,
                CourseCategoryId = _categoryId
            };

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return course.Id;
        }


        public async Task<bool> DeleteCourse(int _courseId)
        {
            var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == _courseId);

            if(course == null)
            {
                return false;
            }

            dbContext.Courses.Remove(course);
            await dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<bool> EditCourse(int _courseId, string _name, string _description, string _imageUrl, int _categoryId)
        {
            var course = await dbContext.Courses.FindAsync(_courseId);

            if (course == null)
            {
                return false;
            }

            course.Name = _name;
            course.Description = _description; 
            course.ImageUrl = _imageUrl;
            course.CourseCategoryId = _categoryId;

            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CourseCategoriesServiceModel>> GetAllCourseCategories()
        {
            return null;
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
