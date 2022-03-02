namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Lectures.Models;

    public class LectureService : ILectureService
    {

        private readonly ELearningSystemDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public LectureService(ELearningSystemDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }


        public async Task<int> AddLectureToCourse(int courseId, string name, string description, IEnumerable<IFormFile> resourceFiles)
        {
            List<Resource> resources = new List<Resource>();           
            foreach (var file in resourceFiles)
            {
                string fullpath = Path.Combine(webHostEnvironment.WebRootPath, file.FileName);
                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            resources = GetResources(resourceFiles);

            var lecture = new Lecture
            {
                Name = name,
                Description = description,
                Resources = resources,
            };

            var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return 0;
            }

            course.Lectures.Add(lecture);
            await dbContext.SaveChangesAsync();

            return lecture.Id;
        }


        public async Task<(bool, int)> DeleteLecture(int lectureId)
        {
            var lecture = await dbContext.Lectures.FindAsync(lectureId);

            if (lecture == null)
            {
                return (false, 0);
            }

            var resources = dbContext.Resources.Where(r => r.LectureId == lectureId).ToList();
            lecture.Resources = resources;

            foreach (var resource in lecture.Resources)
            {
                dbContext.Resources.Remove(resource);
            }

            dbContext.Lectures.Remove(lecture);
            await dbContext.SaveChangesAsync();

            return (true, lecture.CourseId);
        }


        public async Task<bool> EditLecture(int lectureId, string name, string description, IEnumerable<IFormFile> resourceFiles)
        {
            List<Resource> resources = new List<Resource>();
            
            foreach (var file in resourceFiles)
            {
                string fullpath = Path.Combine(webHostEnvironment.WebRootPath, file.FileName);
                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            resources = GetResources(resourceFiles);
            var lecture = dbContext.Lectures.Find(lectureId);

            if (lecture == null)
            {
                return false;
            }

            lecture.Name = name;
            lecture.Description = description;

            foreach (var resourceItem in resources)
            {
                lecture.Resources.Add(resourceItem);
            }
           
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<LectureServiceModel> GetLectureById(int lectureId)
        {
            return await dbContext
                             .Lectures
                             .Where(l => l.Id == lectureId)
                             .Select(l => new LectureServiceModel
                             {
                                 Id = l.Id,
                                 Name = l.Name,
                                 Description = l.Description,
                                 Resources = l.Resources.ToArray(),
                             })
                             .FirstOrDefaultAsync();
        }


        public async Task<LectureDetailsServiceModel> GetLectureDetails(int lectureId)
        {
            return await dbContext
                 .Lectures
                 .Where(l => l.Id == lectureId)
                 .Select(x => new LectureDetailsServiceModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Description = x.Description,
                     ResourceUrls = x.Resources.Select(x => x.Name).ToArray()
                 })
                 .FirstOrDefaultAsync();
        }


        private List<Resource> GetResources(IEnumerable<IFormFile> resourceFiles)
        {
            List<Resource> resources = new List<Resource>();
            int tempResourceId = 1;

            foreach (var resourceFile in resourceFiles)
            {
                if (resourceFile.ContentType == "application/pdf")
                    tempResourceId = 3;
                else if (resourceFile.ContentType == "video/mp4")
                    tempResourceId = 2;
                else
                    tempResourceId = 1;

                resources.Add(new Resource { Name = resourceFile.FileName, ResourceTypeId = tempResourceId });
            }

            return resources;
        }
    }
}
