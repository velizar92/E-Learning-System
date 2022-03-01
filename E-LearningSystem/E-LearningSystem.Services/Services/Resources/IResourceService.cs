using E_LearningSystem.Services.Services.Resources.Models;

namespace E_LearningSystem.Services.Services
{
    public interface IResourceService
    {
        Task<bool> DeleteResource(int _resourceId);

        bool CheckIfResourceTypeExists(int _resourceTypeId);

        Task<IEnumerable<AllResourcesServiceModel>> GetMyResources(string _userId);

    }
}
