namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
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


        public async Task<IEnumerable<AllResourcesServiceModel>> GetMyResources(string userId)
        {
            List<AllResourcesServiceModel> myResources = new List<AllResourcesServiceModel>();

            var myCourses = await dbContext
                                    .Courses
                                    .Where(c => c.UserId == userId)
                                    .Include(l => l.Lectures)
                                    .ThenInclude(l => l.Resources)
                                    .ToListAsync();
            
           
            foreach (var course in myCourses)
            {
                foreach (var lecture in course.Lectures)
                {
                    foreach (var resource in lecture.Resources)
                    {                    
                        myResources.Add(new AllResourcesServiceModel 
                        {
                            ResourceName = resource.Name,                           
                        });
                    }
                }
            }

            return myResources;
        }


    }
}
