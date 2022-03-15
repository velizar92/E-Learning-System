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
    using Microsoft.AspNetCore.Identity;

    public class CourseService : ICourseService
    {
        private readonly ELearningSystemDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<User> userManager;

        public CourseService(ELearningSystemDbContext dbContext, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
        }

        public bool CheckIfCourseCategoryExists(int _categoryId)
        {
            if (dbContext.CourseCategories.Any(c => c.Id == _categoryId) == false)
            {
                return false;
            }

            return true;
        }


        public async Task<int> CreateCourse(string userId, string name, string description, double price, int categoryId, IFormFile pictureFile)
        {

            string detailPath = Path.Combine(@"\assets\img\courses", pictureFile.FileName);
            using (var stream = new FileStream(webHostEnvironment.WebRootPath + detailPath, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            Course course = new Course()
            {
                UserId = userId,
                Name = name,
                Description = description,
                ImageUrl = pictureFile.FileName,
                Price = price,
                CourseCategoryId = categoryId
            };

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
            user.Courses.Add(course);           
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

            var lectures = dbContext.Lectures.Where(r => r.CourseId == courseId).ToList();

            foreach (var lecture in lectures)
            {
                var resources = dbContext.Resources.Where(r => r.LectureId == lecture.Id).ToList();
                foreach (var resource in resources)
                {
                    File.Delete(Path.Combine(webHostEnvironment.WebRootPath, resource.Name));
                    dbContext.Resources.Remove(resource);
                }

                dbContext.Lectures.Remove(lecture);
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

            var trainerUsers = await userManager.GetUsersInRoleAsync("Trainer");

            return await dbContext
                            .Courses
                            .Select(c => new AllCoursesServiceModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Description = c.Description,
                                Price = c.Price,
                                ImageUrl = c.ImageUrl,
                                CategoryName = c.CourseCategory.Name
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
                                Price = c.Price,
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
                               AssignedStudents = x.AssignedStudents,
                               Price = x.Price,
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
                               CategoryName = c.CourseCategory.Name
                           })
                           .Take(count)
                           .ToListAsync();
        }


        
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
                                 CategoryName = c.CourseCategory.Name
                             })
                             .ToListAsync();          
        }

       
    }
}
