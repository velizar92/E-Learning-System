namespace E_LearningSystem.Services.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Data.Data.Models;
    using E_LearningSystem.Services.Services.Courses.Models;
    using E_LearningSystem.Services.Services.Lectures.Models;


    public class CourseService : ICourseService
    {
        private readonly ELearningSystemDbContext dbContext;
      
        public CourseService(ELearningSystemDbContext dbContext)
        {
            this.dbContext = dbContext;          
        }


        public bool CheckIfCourseCategoryExists(int _categoryId)
        {
            if (this.dbContext.CourseCategories.Any(c => c.Id == _categoryId) == false)
            {
                return false;
            }

            return true;
        }


        public async Task<int> CreateCourse(string userId, int trainerId, string name, string description, double price, int categoryId, string pictureFileName)
        {
            Course course = new Course()
            {
                TrainerId = trainerId,
                Name = name,
                Description = description,
                ImageUrl = pictureFileName,
                Price = price,
                CourseCategoryId = categoryId
            };

            this.dbContext.Courses.Add(course);
            await this.dbContext.SaveChangesAsync();

            var courseData = this.dbContext.Courses.FirstOrDefault(c => c.TrainerId == trainerId && c.Name == name);
            var user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.CourseUsers.Add(new CourseUser { CourseId = courseData.Id, UserId = userId });

            await this.dbContext.SaveChangesAsync();

            return courseData.Id;
        }


        public async Task<(bool, string)> DeleteCourse(int courseId)
        {
            var course = await this.dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return (false, "Unknown course.");
            }

            var lectures = this.dbContext.Lectures.Where(r => r.CourseId == courseId).ToList();

            foreach (var lecture in lectures)
            {
                var resources = this.dbContext.Resources.Where(r => r.LectureId == lecture.Id).ToList();
                foreach (var resource in resources)
                {                  
                    dbContext.Resources.Remove(resource);
                }

                this.dbContext.Lectures.Remove(lecture);
            }

            this.dbContext.Courses.Remove(course);
            await this.dbContext.SaveChangesAsync();

            return (true, null);
        }


        public async Task<(bool, string)> EditCourse(int courseId, string name, string description, double price, int categoryId, string pictureFileName)
        {
            var course = await this.dbContext.Courses.FindAsync(courseId);

            if (course == null)
            {
                return (false, "Unknown course.");
            }

            course.ImageUrl = pictureFileName;
            course.Name = name;
            course.Description = description;
            course.Price = price;
            course.CourseCategoryId = categoryId;

            await this.dbContext.SaveChangesAsync();

            return (true, null);
        }


        public async Task<IEnumerable<CourseCategoriesServiceModel>> GetAllCourseCategories()
        {
            return await this.dbContext
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
            return await this.dbContext
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
            var course = await this.dbContext
                                        .Courses
                                        .Where(c => c.Id == id)
                                        .Select(c => new CourseServiceModel
                                        {
                                            Id = c.Id,
                                            Name = c.Name,
                                            Description = c.Description,
                                            ImageUrl = c.ImageUrl,
                                            Price = c.Price,
                                            CategoryId = c.CourseCategoryId
                                        })
                                        .FirstOrDefaultAsync();

            return course;
        }


        public async Task<int> GetCourseCreatorId(int courseId)
        {
            var course = await this.dbContext.Courses.Where(c => c.Id == courseId).FirstOrDefaultAsync();

            if (course == null)
            {
                return -1;
            }

            return course.TrainerId;
        }


        public async Task<CourseDetailsServiceModel> GetCourseDetails(int courseId)
        {
            return await this.dbContext
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
            return await this.dbContext
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
            return await this.dbContext
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
            return await this.dbContext
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
