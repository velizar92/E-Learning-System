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
    using E_LearningSystem.Data.Data.Models;

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


        public async Task<int> CreateCourse(string userId, int trainerId, string name, string description, double price, int categoryId, IFormFile pictureFile)
        {
            string detailPath = Path.Combine(@"\assets\img\courses", pictureFile.FileName);
            using (var stream = new FileStream(webHostEnvironment.WebRootPath + detailPath, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            Course course = new Course()
            {
                TrainerId = trainerId,
                Name = name,
                Description = description,
                ImageUrl = pictureFile.FileName,
                Price = price,
                CourseCategoryId = categoryId
            };

            dbContext.Courses.Add(course);
            await dbContext.SaveChangesAsync();

            var courseData = dbContext.Courses.FirstOrDefault(c => c.TrainerId == trainerId && c.Name == name);                     
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);      
            user.CourseUsers.Add(new CourseUser {CourseId = courseData.Id, UserId = userId});

            await dbContext.SaveChangesAsync();

            return courseData.Id;
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


        public async Task<bool> EditCourse(int courseId, string name, string description, double price, int categoryId, IFormFile pictureFile)
        {
            var course = await dbContext.Courses.FindAsync(courseId);

            if (course == null)
            {
                return false;
            }

            if(pictureFile != null)
            {
                string detailPath = Path.Combine(@"\assets\img\courses", pictureFile.FileName);
                using (var stream = new FileStream(webHostEnvironment.WebRootPath + detailPath, FileMode.Create))
                {
                    await pictureFile.CopyToAsync(stream);
                }
                course.ImageUrl = pictureFile.FileName;
            }
          
            course.Name = name;
            course.Description = description;
            course.Price = price;
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
                                TrainerId = c.TrainerId,
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
                                Id = c.Id,
                                Name = c.Name,
                                Description= c.Description,
                                ImageUrl = c.ImageUrl,
                                Price = c.Price,
                                CategoryId = c.CourseCategoryId
                            })
                            .FirstOrDefaultAsync();

            return course;
        }


        public async Task<int> GetCourseCreatorId(int courseId)
        {
            var course = await dbContext.Courses.Where(c => c.Id == courseId).FirstOrDefaultAsync();

            return course.TrainerId;
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
                               Trainer = x.Trainer,
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
                           .Include(c => c.Trainer)
                           .Select(c => new LatestCoursesServiceModel
                           {
                               Id = c.Id,
                               Name = c.Name,
                               Description = c.Description,
                               Price = c.Price,
                               ImageUrl = c.ImageUrl,
                               CategoryName = c.CourseCategory.Name,
                               Trainer = c.Trainer,
                               AssignedStudents = c.AssignedStudents,
                           })
                           .Take(count)
                           .ToListAsync();
        }


        
        public async Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(string userId)
        {
            return await dbContext
                             .Courses
                             .Where(c => c.CourseUsers.Any(cu => cu.UserId == userId))
                             .Select(c => new AllCoursesServiceModel
                             {
                                 Id = c.Id,
                                 TrainerId = c.TrainerId,
                                 Name = c.Name,
                                 Description = c.Description,
                                 Price = c.Price,
                                 ImageUrl = c.ImageUrl,
                                 CategoryName = c.CourseCategory.Name,
                                 AssignedStudents = c.AssignedStudents
                             })
                             .ToListAsync();
        }

        public async Task<IEnumerable<AllCoursesServiceModel>> GetMyCourses(int trainerId)
        {
            return await dbContext
                             .Courses
                             .Where(c => c.TrainerId == trainerId)
                             .Select(c => new AllCoursesServiceModel
                             {
                                 Id = c.Id,
                                 TrainerId = c.TrainerId,
                                 Name = c.Name,
                                 Description = c.Description,
                                 Price = c.Price,
                                 ImageUrl = c.ImageUrl,
                                 CategoryName = c.CourseCategory.Name,
                                 AssignedStudents = c.AssignedStudents
                             })
                             .ToListAsync();
          
        }


    }
}
