using E_LearningSystem.Services.Services.Resources.Models;

namespace E_LearningSystem.Services.Services
{
    public interface IResourceService
    {
        Task<bool> DeleteResource(int resourceId);

        bool CheckIfResourceTypeExists(int resourceTypeId);

        Task<IEnumerable<AllResourcesServiceModel>> GetMyResources(string userId);

    }
}
