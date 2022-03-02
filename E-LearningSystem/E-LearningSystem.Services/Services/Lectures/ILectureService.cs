namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Services.Services.Lectures.Models;
    using Microsoft.AspNetCore.Http;

    public interface ILectureService
    {
        Task<int> AddLectureToCourse(int courseId, string name, string description, IEnumerable<IFormFile> resourceFiles);

        Task<bool> EditLecture(int lectureId, string name, string description, IEnumerable<IFormFile> resourceFiles);

        Task<(bool, int)> DeleteLecture(int lectureId);

        Task<LectureServiceModel> GetLectureById(int lectureId);

        Task<LectureDetailsServiceModel> GetLectureDetails(int lectureId);
       
    }
}
