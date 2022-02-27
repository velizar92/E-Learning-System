namespace E_LearningSystem.Services.Services
{
    using Microsoft.AspNetCore.Http;

    public interface ILectureService
    {
        Task<int> AddLectureToCourse(int _courseId, string _name, string _description, IEnumerable<IFormFile> _resourceFiles);

        Task<bool> EditLecture(int _lectureId, string _name, string _description, IEnumerable<IFormFile> _resourceFiles);

        Task<(bool, int?)> DeleteLecture(int _lectureId);      

        Task<LectureDetailsServiceModel> GetLectureDetails(int _lectureId);
       
    }
}
