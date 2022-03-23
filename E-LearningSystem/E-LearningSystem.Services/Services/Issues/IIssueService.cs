namespace E_LearningSystem.Services.Services
{
    public interface IIssueService
    {
        Task<int> CreateIssue(string userId, int courseId, string title, string description);

        Task<bool> EditIssue(int issueId, string title, string description);

        Task<bool> DeleteIssue(int issueId);

        Task<IssueDetailsServiceModel> GetIssueDetails(int issueId);

        Task<IEnumerable<AllIssuesServiceModel>> GetAllReportedIssues();

        Task<IEnumerable<AllIssuesServiceModel>> GetAllReportedIssuesForCourse(int courseId);

        Task<IEnumerable<AllIssuesServiceModel>> GetMyReportedIssues(string userId, int courseId);
    }
}
