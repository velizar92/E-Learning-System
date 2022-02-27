namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;

    public class IssueService : IIssueService
    {
        private readonly ELearningSystemDbContext dbContext;

        public IssueService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }


        public async Task<int> CreateIssue(string _userId, int _courseId, string _title, string _description)
        {
            var course = await dbContext
                             .Courses
                             .Where(c => c.Id == _courseId)
                             .FirstOrDefaultAsync();

            if (course == null)
            {
                return 0;
            }

            var issue = new Issue()
            {
                Title = _title,
                Description = _description,
                CourseId = _courseId,
                UserId = _userId,
            };

            course.Issues.Add(issue);           
            await dbContext.SaveChangesAsync();

            return issue.Id;
        }


        public async Task<bool> DeleteIssue(int _issueId)
        {
            var issue = await dbContext.Issues.FirstOrDefaultAsync(i => i.Id == _issueId);

            if (issue == null)
            {
                return false;
            }

            dbContext.Issues.Remove(issue);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> EditIssue(int _issueId, string _title, string _description)
        {
            var issue = await dbContext.Issues.FirstOrDefaultAsync(i => i.Id == _issueId);

            if (issue == null)
            {
                return false;
            }

            issue.Title = _title;
            issue.Description = _description;

            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<AllIssuesServiceModel>> GetAllReportedIssues()
        {
            return await dbContext
                            .Issues
                            .Select(i => new AllIssuesServiceModel
                            {
                                Title = i.Title,
                                Description = i.Description,
                            })
                            .ToListAsync();
        }


        public async Task<IssueDetailsServiceModel> GetIssueDetails(int _issueId)
        {
            var issue = await dbContext.Issues.FirstOrDefaultAsync(i => i.Id == _issueId);

            var issueDetails = new IssueDetailsServiceModel
            {
                Title = issue.Title,
                Description = issue.Description,
            };

            return issueDetails;
        }


        public async Task<IEnumerable<AllIssuesServiceModel>> GetMyReportedIssues(string _userId)
        {
            return await dbContext
                            .Issues
                            .Where(i => i.UserId == _userId)
                            .Select(i => new AllIssuesServiceModel
                            {
                                Title = i.Title,
                                Description = i.Description,
                            })
                            .ToListAsync();
        }
    }
}
