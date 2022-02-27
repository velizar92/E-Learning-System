namespace E_LearningSystem.Services.Services
{
    public interface IResourceService
    {
        Task<bool> DeleteResource(int _resourceId);

        bool CheckIfResourceTypeExists(int _resourceTypeId);

    }
}
