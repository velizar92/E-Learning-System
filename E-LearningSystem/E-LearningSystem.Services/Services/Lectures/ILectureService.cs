namespace E_LearningSystem.Services.Services
{
    using Microsoft.AspNetCore.Http;

    public interface ILectureService
    {
        Task<int> AddLectureToCourse(int _courseId, string _name, string _description, IEnumerable<IFormFile> _resourceFiles);

        Task<bool> EditLecture(int _lectureId, string _name, string _description, string _imageUrl, int _categoryId);

        Task<bool> DeleteLecture(int _lectureId);      

        Task<LectureDetailsServiceModel> GetLectureDetails(int _lectureId);

        Task<IEnumerable<AllLecturesServiceModel>> GetAllLectures(int _courseId);
   
    }
}
