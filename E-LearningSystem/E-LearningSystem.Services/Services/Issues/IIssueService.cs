namespace E_LearningSystem.Services.Services
{
    public interface IIssueService
    {
        Task<int> CreateIssue(string _userId, int _courseId, string _title, string _description);

        Task<bool> EditIssue(int _issueId, string _title, string _description);

        Task<bool> DeleteIssue(int _issueId);

        Task<IssueDetailsServiceModel> GetIssueDetails(int _issueId);

        Task<IEnumerable<AllIssuesServiceModel>> GetAllReportedIssues();

        Task<IEnumerable<AllIssuesServiceModel>> GetMyReportedIssues(string _userId);
    }
}
