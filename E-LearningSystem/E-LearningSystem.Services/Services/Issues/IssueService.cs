namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Issues.Models;

    public class IssueService : IIssueService
    {
        private readonly ELearningSystemDbContext dbContext;

        public IssueService(ELearningSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<(bool, string)> CreateIssue(string userId, int courseId, string title, string description)
        {
            var course = await this.dbContext
                             .Courses
                             .Where(c => c.Id == courseId)
                             .FirstOrDefaultAsync();

            if (course == null)
            {
                return (false, "Invalid course id.");
            }

            var issue = new Issue()
            {
                Title = title,
                Description = description,
                CourseId = courseId,
                UserId = userId,
            };

            course.Issues.Add(issue);           
            await this.dbContext.SaveChangesAsync();

            return (true, null);
        }


        public async Task<(bool, string)> DeleteIssue(int issueId)
        {
            var issue = await this.dbContext.Issues.FirstOrDefaultAsync(i => i.Id == issueId);

            if (issue == null)
            {
                return (false, "Invalid issue id.");
            }

            this.dbContext.Issues.Remove(issue);
            await this.dbContext.SaveChangesAsync();

            return (true, null);
        }


        public async Task<(bool, string)> EditIssue(int issueId, string title, string description)
        {
            var issue = await this.dbContext.Issues.FirstOrDefaultAsync(i => i.Id == issueId);

            if (issue == null)
            {
                return (false, "Invalid issue id.");
            }

            issue.Title = title;
            issue.Description = description;

            await this.dbContext.SaveChangesAsync();

            return (true, null);
        }


        public async Task<IEnumerable<AllIssuesServiceModel>> GetAllReportedIssues()
        {
            return await this.dbContext
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
            return await this.dbContext
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
            var issue = await this.dbContext.Issues.FirstOrDefaultAsync(i => i.Id == issueId);

            var issueDetails = this.dbContext
                                        .Issues
                                        .Where(i => i.Id == issueId)
                                        .Select(i => new IssueDetailsServiceModel
                                        {
                                            CourseId = issue.CourseId,
                                            Title = issue.Title,
                                            Description = issue.Description,
                                        })
                                        .FirstOrDefault();

            return issueDetails;
        }


        public async Task<IEnumerable<AllIssuesServiceModel>> GetMyReportedIssues(string userId, int courseId)
        {
            return await this.dbContext
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
