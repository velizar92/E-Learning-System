namespace E_LearningSystem.Services.Services
{
    public interface IIssueService
    {
        Task<int> CreateIssue(int _courseId, string _title, string _description);

        Task<bool> EditIssue(int _courseId, int _issueId, string _title, string _description);

        Task<bool> DeleteIssue(int _issueId);

        Task<IssueDetailsServiceModel> GetIssueDetails(int _issueId);

        IEnumerable<Task<AllIssuesServiceModel>> GetAllReportedIssues();

        IEnumerable<Task<MyIssuesServiceModel>> GetMyReportedIssues(string _userId);
    }
}
