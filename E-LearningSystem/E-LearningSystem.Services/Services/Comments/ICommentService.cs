namespace E_LearningSystem.Services.Services
{
    public interface ICommentService
    {
        Task<int> CreateComment(int _lectureId, string _userId, string _content);

        Task<bool> EditComment(int _commentId, string _content);

        Task<bool> DeleteComment(int _commentId);

        Task<IEnumerable<AllLectureCommentsServiceModel>> GetLectureComments(int _lectureId);
        
    }
}
