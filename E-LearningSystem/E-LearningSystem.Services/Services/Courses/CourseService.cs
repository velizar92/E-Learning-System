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

        public CourseService(ELearningSystemDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;       
        }

        public bool CheckIfCourseCategoryExists(int _categoryId)
        {
            if (dbContext.CourseCategories.Any(c => c.Id == _categoryId) == false)
            {
                return false;
            }

            return true;
        }


        public async Task<int> CreateCourse(string userId, string name, string description, int categoryId, IFormFile pictureFile)
        {

            string fullpath = Path.Combine(webHostEnvironment.WebRootPath, pictureFile.FileName);
            using (var stream = new FileStream(fullpath, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            Course course = new Course()
            {
                UserId = userId,
                Name = name,
                Description = description,
                ImageUrl = pictureFile.FileName,
                CourseCategoryId = categoryId
            };

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return course.Id;
        }


        public async Task<bool> DeleteCourse(int courseId)
        {
            var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return false;
            }

            dbContext.Courses.Remove(course);
            await dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<bool> EditCourse(int courseId, string name, string description, int categoryId, IFormFile pictureFile)
        {
            var course = await dbContext.Courses.FindAsync(courseId);

            if (course == null)
            {
                return false;
            }

            string fullpath = Path.Combine(webHostEnvironment.WebRootPath, pictureFile.FileName);
            using (var stream = new FileStream(fullpath, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            course.Name = name;
            course.Description = description;
            course.ImageUrl = pictureFile.FileName;
            course.CourseCategoryId = categoryId;

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

        public async Task<CourseServiceModel> GetCourseById(int id)
        {
            var course = await dbContext
                            .Courses    
                            .Where(c => c.Id == id)
                            .Select(c => new CourseServiceModel
                            { 
                                Name = c.Name,
                                Description= c.Description,
                                ImageUrl = c.ImageUrl,
                                CategoryId = c.CourseCategoryId
                            })
                            .FirstOrDefaultAsync();

            return course;
        }


        public async Task<CourseDetailsServiceModel> GetCourseDetails(int courseId)
        {       
            return await dbContext
                           .Courses
                           .Where(c => c.Id == courseId)
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


        public async Task<IEnumerable<LatestCoursesServiceModel>> GetLatestCourses(int count)
        {
            return await dbContext
                           .Courses
                           .OrderByDescending(c => c.CreatedOn)
                           .Select(c => new LatestCoursesServiceModel
                           {
                               Id = c.Id,
                               Name = c.Name,
                               Description = c.Description,
                               Price = c.Price,
                               ImageUrl = c.ImageUrl,
                           })
                           .Take(count)
                           .ToListAsync();
        }


        //Get my course as a normal user ("Learner")
        public async Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(string userId)
        {
            return await dbContext
                             .Courses
                             .Where(c => c.UserId == userId)
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
