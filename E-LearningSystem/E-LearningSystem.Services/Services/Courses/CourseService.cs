namespace E_LearningSystem.Services.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Courses.Models;
    using E_LearningSystem.Services.Services.Lectures.Models;

    public class CourseService : ICourseService
    {
        private readonly ELearningSystemDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;      

        public CourseService(ELearningSystemDbContext _dbContext, IWebHostEnvironment _webHostEnvironment)
        {
            this.dbContext = _dbContext;
            this.webHostEnvironment = _webHostEnvironment;       
        }

        public bool CheckIfCourseCategoryExists(int _categoryId)
        {
            if (dbContext.CourseCategories.Any(c => c.Id == _categoryId) == false)
            {
                return false;
            }

            return true;
        }


        public async Task<int> CreateCourse(string userId, string _name, string _description, int _categoryId, IFormFile _pictureFile)
        {

            string fullpath = Path.Combine(webHostEnvironment.WebRootPath, _pictureFile.FileName);
            using (var stream = new FileStream(fullpath, FileMode.Create))
            {
                await _pictureFile.CopyToAsync(stream);
            }

            Course course = new Course()
            {
                UserId = userId,
                Name = _name,
                Description = _description,
                ImageUrl = _pictureFile.FileName,
                CourseCategoryId = _categoryId
            };

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return course.Id;
        }


        public async Task<bool> DeleteCourse(int _courseId)
        {
            var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == _courseId);

            if (course == null)
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
            return await dbContext
                            .CourseCategories
                            .Select(c => new CourseCategoriesServiceModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                            })
                           .ToListAsync();
        } 

                 

        public async Task<IEnumerable<AllCoursesServiceModel>> GetAllCourses()
        {
            return await dbContext
                            .Courses
                            .Select(c => new AllCoursesServiceModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Description = c.Description,
                                Price = c.Price,
                                ImageUrl = c.ImageUrl,
                            })
                            .ToListAsync();

        }


        public async Task<CourseDetailsServiceModel> GetCourseDetails(int _courseId)
        {       
            return await dbContext
                           .Courses
                           .Where(c => c.Id == _courseId)
                           .Select(x => new CourseDetailsServiceModel
                           {
                               Id = x.Id,
                               Name = x.Name,
                               Description = x.Description,
                               ImageUrl = x.ImageUrl,
                               Lectures = x.Lectures.Select(x => new LectureServiceModel
                               {
                                   Id = x.Id,
                                   Name = x.Name,
                                   Description = x.Description
                               })
                               .ToList()
                           })
                           .FirstOrDefaultAsync();          
        }


        public async Task<IEnumerable<LatestCoursesServiceModel>> GetLatestCourses(int _count)
        {
            return await dbContext
                           .Courses
                           .Select(c => new LatestCoursesServiceModel
                           {
                               Id = c.Id,
                               Name = c.Name,
                               Description = c.Description,
                               Price = c.Price,
                               ImageUrl = c.ImageUrl,
                           })
                           .Take(_count)
                           .ToListAsync();
        }


        //Get my course as a normal user ("Learner")
        public async Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(string _userId)
        {
            return await dbContext
                             .Courses
                             .Where(c => c.UserId == _userId)
                             .Select(c => new AllCoursesServiceModel
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Description = c.Description,
                                 Price = c.Price,
                                 ImageUrl = c.ImageUrl,
                             })
                             .ToListAsync();          
        }


        //Get my course as a normal user ("Trainer")
        public async Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(int _trainerId)
        {
            return await dbContext
                             .Courses
                             .Where(c => c.TrainerId == _trainerId)
                             .Select(c => new AllCoursesServiceModel
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Description = c.Description,
                                 Price = c.Price,
                                 ImageUrl = c.ImageUrl,
                             })
                             .ToListAsync();
        }
    }
}
