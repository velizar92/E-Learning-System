namespace E_LearningSystem.Services.Services.Storage
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    public class FileStorageService : IStorageService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task SaveFile(string destinationFolderPath, IFormFile file)
        {
            string detailPath = Path.Combine(destinationFolderPath, file.FileName);
            using (var stream = new FileStream(webHostEnvironment.WebRootPath + detailPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public async Task SaveFiles(string destinationFolderPath, IEnumerable<IFormFile> files)
        {
            foreach (var file in files)
            {
                string detailPath = Path.Combine(destinationFolderPath, file.FileName);
                using (var stream = new FileStream(webHostEnvironment.WebRootPath + detailPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }
    }
}
