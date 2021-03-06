namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Services.Services.Comments.Models;

    public interface ICommentService
    {
        Task<bool> CreateComment(int lectureId, string userId, string content);      

        Task<(bool, int?)> DeleteComment(int commentId);

        Task<CommentServiceModel> GetCommentById(int commentId);

        Task<IEnumerable<AllLectureCommentsServiceModel>> GetLectureComments(int lectureId);
        
    }
}
