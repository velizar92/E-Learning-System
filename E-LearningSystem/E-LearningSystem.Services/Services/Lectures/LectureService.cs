namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using E_LearningSystem.Data.Models;

    public class LectureService : ILectureService
    {

        private readonly ELearningSystemDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public LectureService(ELearningSystemDbContext _dbContext, IWebHostEnvironment _webHostEnvironment)
        {
            this.dbContext = _dbContext;
            this.webHostEnvironment = _webHostEnvironment;
        }


        public async Task<int> AddLectureToCourse(int _courseId, string _name, string _description, IEnumerable<IFormFile> _resourceFiles)
        {
            List<Resource> resources = new List<Resource>();           
            foreach (var file in _resourceFiles)
            {
                string fullpath = Path.Combine(webHostEnvironment.WebRootPath, file.FileName);
                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            resources = GetResources(_resourceFiles);

            var lecture = new Lecture
            {
                Name = _name,
                Description = _description,
                Resources = resources,
            };

            var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == _courseId);

            if (course == null)
            {
                return 0;
            }

            course.Lectures.Add(lecture);
            await dbContext.SaveChangesAsync();

            return lecture.Id;
        }


        public async Task<(bool, int?)> DeleteLecture(int _lectureId)
        {
            var lecture = await dbContext.Lectures.FindAsync(_lectureId);

            if (lecture == null)
            {
                return (false, null);
            }

            var resources = dbContext.Resources.Where(r => r.LectureId == _lectureId).ToList();
            lecture.Resources = resources;

            foreach (var resource in lecture.Resources)
            {
                dbContext.Resources.Remove(resource);
            }

            dbContext.Lectures.Remove(lecture);
            await dbContext.SaveChangesAsync();

            return (true, lecture.CourseId);
        }


        public async Task<bool> EditLecture(int _lectureId, string _name, string _description, IEnumerable<IFormFile> _resourceFiles)
        {
            List<Resource> resources = new List<Resource>();
            
            foreach (var file in _resourceFiles)
            {
                string fullpath = Path.Combine(webHostEnvironment.WebRootPath, file.FileName);
                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            resources = GetResources(_resourceFiles);
            var lecture = dbContext.Lectures.Find(_lectureId);

            if (lecture == null)
            {
                return false;
            }

            lecture.Name = _name;
            lecture.Description = _description;
            lecture.Resources = resources;

            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<LectureDetailsServiceModel> GetLectureDetails(int _lectureId)
        {
            return await dbContext
                 .Lectures
                 .Where(l => l.Id == _lectureId)
                 .Select(x => new LectureDetailsServiceModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Description = x.Description,
                     ResourceUrls = x.Resources.Select(x => x.Name).ToArray()
                 })
                 .FirstOrDefaultAsync();
        }


        private List<Resource> GetResources(IEnumerable<IFormFile> _resourceFiles)
        {
            List<Resource> resources = new List<Resource>();
            int tempResourceId = 1;

            foreach (var resourceFile in _resourceFiles)
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
