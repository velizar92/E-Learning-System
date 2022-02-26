namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    public class LectureService : ILectureService
    {

        private readonly ELearningSystemDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public LectureService(ELearningSystemDbContext _dbContext, IWebHostEnvironment _webHostEnvironment)
        {
            this.dbContext = _dbContext;
            this.webHostEnvironment  = _webHostEnvironment;
        }


        public Task<int> AddLectureToCourse(int _courseId, string _name, string _description, IEnumerable<IFormFile> _resourceFiles)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLecture(int _lectureId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditLecture(int _lectureId, string _name, string _description, string _imageUrl, int _categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AllLecturesServiceModel>> GetAllLectures(int _courseId)
        {
            throw new NotImplementedException();
        }

        public Task<LectureDetailsServiceModel> GetLectureDetails(int _lectureId)
        {
            throw new NotImplementedException();
        }
    }
}
