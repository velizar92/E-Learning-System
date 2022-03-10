namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Services.Services.Resources;
    using E_LearningSystem.Services.Services.Resources.Models;

    public interface IResourceService
    {
        Task<bool> DeleteResource(int resourceId);

        bool CheckIfResourceTypeExists(int resourceTypeId);

        Task<IEnumerable<string>> GetAllResourceTypes();

        Task<ResourceQueryServiceModel> GetMyResources(
            string userId,
            string resourceType,
            string searchTerm,
            int currentPage,
            int resourcesPerPage);

    }
}
