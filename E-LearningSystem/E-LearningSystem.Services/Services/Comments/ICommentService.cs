namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Services.Services.Comments.Models;

    public interface ICommentService
    {
        Task<int> CreateComment(int lectureId, string userId, string content);

        Task<(bool, int?)> EditComment(int commentId, string content);

        Task<(bool, int?)> DeleteComment(int commentId);

        Task<CommentServiceModel> GetCommentById(int commentId);

        Task<IEnumerable<AllLectureCommentsServiceModel>> GetLectureComments(int lectureId);
        
    }
}
