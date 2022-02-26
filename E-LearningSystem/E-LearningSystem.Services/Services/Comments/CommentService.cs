namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;

    public class CommentService : ICommentService
    {
        private readonly ELearningSystemDbContext dbContext;

        public CommentService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }


        public Task<int> CreateComment(int _lectureId, string _userId, string _content)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteComment(int _commentId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditComment(int _commentId, string _content)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task<AllLectureCommentsServiceModel>> GetLectureComments(string _lectureId)
        {
            throw new NotImplementedException();
        }
    }
}
