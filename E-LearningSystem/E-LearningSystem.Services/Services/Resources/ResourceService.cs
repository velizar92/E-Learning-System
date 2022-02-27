namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
   
    public class ResourceService : IResourceService
    {
        private readonly ELearningSystemDbContext dbContext;
       
        public ResourceService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;          
        }

        public bool CheckIfResourceTypeExists(int _resourceTypeId)
        {
            if (dbContext.ResourceTypes.Any(r => r.Id == _resourceTypeId) == false)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteResource(int _resourceId)
        {
            var resource = await dbContext.Resources.FirstOrDefaultAsync(r => r.Id == _resourceId);

            if (resource == null)
            {
                return false;
            }

            dbContext.Resources.Remove(resource);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
