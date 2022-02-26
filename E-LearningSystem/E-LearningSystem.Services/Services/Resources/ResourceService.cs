namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;

    public class ResourceService : IResourceService
    {

        private readonly ELearningSystemDbContext dbContext;
       
        public ResourceService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;          
        }

        public Task<bool> CheckIfResourceTypeExists(int _resourceTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteResource(int _resourceId)
        {
            throw new NotImplementedException();
        }
    }
}
