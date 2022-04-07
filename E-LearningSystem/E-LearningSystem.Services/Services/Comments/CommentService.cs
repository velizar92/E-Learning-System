namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Comments.Models;

    public class CommentService : ICommentService
    {
        private readonly ELearningSystemDbContext dbContext;

        public CommentService(ELearningSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<bool> CreateComment(int lectureId, string userId, string content)
        {
            var lecture = await this.dbContext
                             .Lectures
                             .Where(l => l.Id == lectureId).FirstOrDefaultAsync();

            if (lecture == null)
            {
                return false;
            }

            var comment = new Comment()
            {
                LectureId = lectureId,
                UserId = userId,
                Content = content,
            };

            await this.dbContext.Comments.AddAsync(comment);
            await this.dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<(bool, int?)> DeleteComment(int commentId)
        {       
            var comment = await this.dbContext
                .Comments
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment != null)
            {
                int lectureId = comment.LectureId;
                this.dbContext.Comments.Remove(comment);
                await this.dbContext.SaveChangesAsync();
                return (true, lectureId);
            }

            return (false, null);
        }


        public async Task<(bool, int?)> EditComment(int commentId, string content)
        {
            var comment = await this.dbContext
                            .Comments
                            .Where(c => c.Id == commentId).FirstOrDefaultAsync();

            if (comment != null)
            {
                int lectureId = comment.LectureId;
                await this.dbContext.SaveChangesAsync();
                return (true, lectureId);
            }

            comment.Content = content;

            return (false, null);
        }


        public Task<CommentServiceModel> GetCommentById(int commentId)
        {
            var comment = this.dbContext
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

           var result = await this.dbContext
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
