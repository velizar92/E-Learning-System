namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;

    public class IssueService : IIssueService
    {
        private readonly ELearningSystemDbContext dbContext;

        public IssueService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public Task<int> CreateIssue(int _courseId, string _title, string _description)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteIssue(int _issueId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditIssue(int _courseId, int _issueId, string _title, string _description)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task<AllIssuesServiceModel>> GetAllReportedIssues()
        {
            throw new NotImplementedException();
        }

        public Task<IssueDetailsServiceModel> GetIssueDetails(int _issueId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task<MyIssuesServiceModel>> GetMyReportedIssues(string _userId)
        {
            throw new NotImplementedException();
        }
    }
}
