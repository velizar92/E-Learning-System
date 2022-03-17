namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Resources.Models;

    public class ResourceService : IResourceService
    {
        private readonly ELearningSystemDbContext dbContext;

        public ResourceService(ELearningSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public bool CheckIfResourceTypeExists(int resourceTypeId)
        {
            if (dbContext.ResourceTypes.Any(r => r.Id == resourceTypeId) == false)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<string>> GetAllResourceTypes()
        {
            return await dbContext
                            .ResourceTypes
                            .Select(c => c.Name)
                            .Distinct()
                            .ToListAsync();
        }


        public async Task<bool> DeleteResource(int resourceId)
        {
            var resource = await dbContext.Resources.FirstOrDefaultAsync(r => r.Id == resourceId);

            if (resource == null)
            {
                return false;
            }

            dbContext.Resources.Remove(resource);
            await dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<ResourceQueryServiceModel> GetAllMyResources(
            string userId,
            string resourceType = null,
            string searchTerm = null,         
            int currentPage = 1,
            int resourcesPerPage = int.MaxValue)
        {

            List<Resource> myResources = new List<Resource>();

            var myCourses = await dbContext
                                    .Courses
                                    .Where(c => c.CourseUsers.Any(cu => cu.UserId == userId))
                                    .Include(l => l.Lectures)
                                    .ThenInclude(l => l.Resources)
                                    .ToListAsync();

            foreach (var course in myCourses)
            {
                foreach (var lecture in course.Lectures)
                {
                    foreach (var resource in lecture.Resources)
                    {
                        myResources.Add(resource);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(resourceType))
            {
                myResources = myResources.Where(r => r.Name == resourceType).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                myResources = myResources.Where(r =>
                    (r.Name.ToLower()).Contains(searchTerm.ToLower())).ToList();
            }

            int resourcesCount = myResources.Count;

            myResources = myResources
                .Skip((currentPage - 1) * resourcesPerPage)
                .Take(resourcesPerPage)
                .ToList();

            return new ResourceQueryServiceModel
            {
                Resources = myResources.Select(r => new AllResourcesServiceModel
                {
                    ResourceName = r.Name,
                    LectureName = r.Lecture.Name
                }),

                TotalResources = resourcesCount,
                CurrentPage = currentPage,
                CarsPerPage = resourcesPerPage
            };        
        }


        public async Task<IEnumerable<AllResourcesServiceModel>> GetMyResources(string userId)
        {
            List<Resource> myResources = new List<Resource>();

            var myCourses = await dbContext
                                    .Courses
                                    .Where(c => c.CourseUsers.Any(cu => cu.UserId == userId))
                                    .Include(l => l.Lectures)
                                    .ThenInclude(l => l.Resources)
                                    .ToListAsync();

            foreach (var course in myCourses)
            {
                foreach (var lecture in course.Lectures)
                {
                    foreach (var resource in lecture.Resources)
                    {
                        myResources.Add(resource);
                    }
                }
            }

            var resources = myResources.Select(r => new AllResourcesServiceModel
            {
                ResourceName = r.Name,
                LectureName = r.Lecture.Name
            });

            return resources;

           
        }






    }
}
