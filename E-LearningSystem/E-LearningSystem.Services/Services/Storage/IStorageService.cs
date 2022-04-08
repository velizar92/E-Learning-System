namespace E_LearningSystem.Services.Services.Storage
{
    using Microsoft.AspNetCore.Http;
    public interface IStorageService
    {
        Task SaveFile(string destinationFolderPath, IFormFile file);
        Task SaveFiles(string destinationFolderPath, IEnumerable<IFormFile> files);
    }
}
