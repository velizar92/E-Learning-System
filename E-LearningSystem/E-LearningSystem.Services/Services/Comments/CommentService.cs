namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Comments.Models;

    public class CommentService : ICommentService
    {
        private readonly ELearningSystemDbContext dbContext;

        public CommentService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }


        public async Task<int> CreateComment(int _lectureId, string _userId, string _content)
        {
            var lecture = await dbContext
                             .Lectures
                             .Where(l => l.Id == _lectureId).FirstOrDefaultAsync();

            if (lecture == null)
            {
                return 0;
            }

            var comment = new Comment()
            {
                LectureId = _lectureId,
                UserId = _userId,
                Content = _content,
            };


            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            return comment.Id;
        }


        public async Task<bool> DeleteComment(int _commentId)
        {
            var comment = await dbContext
                .Comments
                .FirstOrDefaultAsync(c => c.Id == _commentId);

            if (comment != null)
            {
                dbContext.Comments.Remove(comment);
                return true;
            }

            return false;
        }


        public async Task<bool> EditComment(int _commentId, string _content)
        {
            var comment = await dbContext
                            .Comments
                            .Where(c => c.Id == _commentId).FirstOrDefaultAsync();

            if (comment == null)
            {
                return false;
            }

            comment.Content = _content;
            await dbContext.SaveChangesAsync();

            return true;
        }


        public Task<CommentServiceModel> GetCommentById(int _commentId)
        {
            var comment = dbContext
                             .Comments
                             .Where(c => c.Id == _commentId)
                             .Select(c => new CommentServiceModel
                             {
                                 Content = c.Content
                             })
                             .FirstOrDefaultAsync();

            return comment;
        }


        public async Task<IEnumerable<AllLectureCommentsServiceModel>> GetLectureComments(int _lectureId)
        {
            return await dbContext
                      .Comments
                      .Where(c => c.LectureId == _lectureId)
                      .Select(c => new AllLectureCommentsServiceModel
                      {
                          Id = c.Id,
                          LectureId = c.LectureId,
                          Content = c.Content
                      })
                      .ToListAsync();

        }


    }
}
