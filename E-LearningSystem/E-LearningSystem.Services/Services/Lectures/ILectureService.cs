namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Lectures.Models; 

    public interface ILectureService
    {
        Task<int> AddLectureToCourse(int courseId, string name, string description, List<Resource> resources);

        Task<bool> EditLecture(int lectureId, string name, string description, List<Resource> resources);

        Task<(bool, int)> DeleteLecture(int lectureId);

        Task<LectureServiceModel> GetLectureById(int lectureId);

        Task<LectureDetailsServiceModel> GetLectureDetails(int lectureId);

        Task<int> GetLectureIdByResourceId(int resourceId);       
    }
}
