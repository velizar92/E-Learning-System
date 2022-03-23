namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;

    public class IssueService : IIssueService
    {
        private readonly ELearningSystemDbContext dbContext;

        public IssueService(ELearningSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<int> CreateIssue(string userId, int courseId, string title, string description)
        {
            var course = await dbContext
                             .Courses
                             .Where(c => c.Id == courseId)
                             .FirstOrDefaultAsync();

            if (course == null)
            {
                return 0;
            }

            var issue = new Issue()
            {
                Title = title,
                Description = description,
                CourseId = courseId,
                UserId = userId,
            };

            course.Issues.Add(issue);           
            await dbContext.SaveChangesAsync();

            return issue.Id;
        }


        public async Task<bool> DeleteIssue(int issueId)
        {
            var issue = await dbContext.Issues.FirstOrDefaultAsync(i => i.Id == issueId);

            if (issue == null)
            {
                return false;
            }

            dbContext.Issues.Remove(issue);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> EditIssue(int issueId, string title, string description)
        {
            var issue = await dbContext.Issues.FirstOrDefaultAsync(i => i.Id == issueId);

            if (issue == null)
            {
                return false;
            }

            issue.Title = title;
            issue.Description = description;

            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<AllIssuesServiceModel>> GetAllReportedIssues()
        {
            return await dbContext
                            .Issues
                            .Select(i => new AllIssuesServiceModel
                            {
                                CourseId = i.CourseId,
                                CourseName = i.Course.Name,
                                IssueId = i.Id,
                                Title = i.Title,
                                Description = i.Description,
                            })
                            .ToListAsync();
        }


        public async Task<IEnumerable<AllIssuesServiceModel>> GetAllReportedIssuesForCourse(int courseId)
        {
            return await dbContext
                            .Issues
                            .Where(i => i.CourseId == courseId)
                            .Select(i => new AllIssuesServiceModel
                            {
                                CourseId = i.CourseId,
                                CourseName = i.Course.Name,
                                IssueId = i.Id,
                                Title = i.Title,
                                Description = i.Description,
                            })
                            .ToListAsync();
        }


        public async Task<IssueDetailsServiceModel> GetIssueDetails(int issueId)
        {
            var issue = await dbContext.Issues.FirstOrDefaultAsync(i => i.Id == issueId);

            var issueDetails = new IssueDetailsServiceModel
            {
                CourseId = issue.CourseId,
                Title = issue.Title,
                Description = issue.Description,
            };

            return issueDetails;
        }


        public async Task<IEnumerable<AllIssuesServiceModel>> GetMyReportedIssues(string userId, int courseId)
        {
            return await dbContext
                            .Issues
                            .Where(i => i.UserId == userId && i.CourseId == courseId)
                            .Select(i => new AllIssuesServiceModel
                            {
                                CourseId = i.CourseId,
                                CourseName = i.Course.Name,
                                IssueId =i.Id,
                                Title = i.Title,
                                Description = i.Description,
                            })
                            .ToListAsync();
        }
    }
}
