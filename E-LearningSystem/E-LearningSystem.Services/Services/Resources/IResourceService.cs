namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Services.Services.Resources.Models;

    public interface IResourceService
    {
        Task<bool> DeleteResource(int resourceId);

        bool CheckIfResourceTypeExists(int resourceTypeId);

        Task<IEnumerable<string>> GetAllResourceTypes();

        Task<IEnumerable<AllResourcesServiceModel>> GetMyResources(string userId);

        Task<ResourceQueryServiceModel> GetAllMyResources(
            string userId,          
            string searchTerm
           );

    }
}
