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


        public async Task<int> CreateComment(int lectureId, string userId, string content)
        {
            var lecture = await dbContext
                             .Lectures
                             .Where(l => l.Id == lectureId).FirstOrDefaultAsync();

            if (lecture == null)
            {
                return 0;
            }

            var comment = new Comment()
            {
                LectureId = lectureId,
                UserId = userId,
                Content = content,
            };

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            return comment.Id;
        }


        public async Task<(bool, int?)> DeleteComment(int commentId)
        {       
            var comment = await dbContext
                .Comments
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment != null)
            {
                int lectureId = comment.LectureId;
                dbContext.Comments.Remove(comment);
                return (true, lectureId);
            }

            return (false, null);
        }


        public async Task<(bool, int?)> EditComment(int commentId, string content)
        {
            var comment = await dbContext
                            .Comments
                            .Where(c => c.Id == commentId).FirstOrDefaultAsync();

            if (comment != null)
            {
                int lectureId = comment.LectureId;
                await dbContext.SaveChangesAsync();
                return (true, lectureId);
            }

            comment.Content = content;

            return (false, null);
        }


        public Task<CommentServiceModel> GetCommentById(int commentId)
        {
            var comment = dbContext
                             .Comments
                             .Where(c => c.Id == commentId)
                             .Select(c => new CommentServiceModel
                             {
                                 Content = c.Content
                             })
                             .FirstOrDefaultAsync();

            return comment;
        }


        public async Task<IEnumerable<AllLectureCommentsServiceModel>> GetLectureComments(int lectureId)
        {

           var result = await dbContext
                                   .Comments
                                   .Where(c => c.LectureId == lectureId)                             
                                   .Select(c => new AllLectureCommentsServiceModel
                                   {
                                       Id = c.Id,
                                       LectureId = lectureId,
                                       Content = c.Content,      
                                       UserId = c.UserId,
                                       CreatedOn = c.CreatedOn,
                                       UpdatedOn = c.UpdatedOn
                                   })
                                   .ToListAsync();

            return result;    
        }

    }
}
