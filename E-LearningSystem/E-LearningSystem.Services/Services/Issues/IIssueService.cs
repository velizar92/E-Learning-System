namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Services.Services.Issues.Models;

    public interface IIssueService
    {
        Task<(bool, string)> CreateIssue(string userId, int courseId, string title, string description);

        Task<(bool, string)> EditIssue(int issueId, string title, string description);

        Task<(bool, string)> DeleteIssue(int issueId);

        Task<IssueDetailsServiceModel> GetIssueDetails(int issueId);

        Task<IEnumerable<AllIssuesServiceModel>> GetAllReportedIssues();

        Task<IEnumerable<AllIssuesServiceModel>> GetAllReportedIssuesForCourse(int courseId);

        Task<IEnumerable<AllIssuesServiceModel>> GetMyReportedIssues(string userId, int courseId);
    }
}
