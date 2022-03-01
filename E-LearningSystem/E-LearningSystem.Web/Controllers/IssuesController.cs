namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity; 
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Issue;

    public class IssuesController : Controller
    {
        private readonly IIssueService issueService;
        private readonly UserManager<User> userManagerService;


        public IssuesController(IIssueService _issueService, UserManager<User> _userManagerService)
        {
            this.issueService = _issueService;
            this.userManagerService = _userManagerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int _courseId, IssueFormModel _issueModel)
        {
            var user = await this.userManagerService.GetUserAsync(HttpContext.User);

            int issueId = await this.issueService.CreateIssue(user.Id, _courseId, _issueModel.Title, _issueModel.Description);

            return RedirectToAction(nameof(MyIssues));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int _commentId)
        {
            var issue = await this.issueService.GetIssueDetails(_commentId);

            IssueFormModel issueFormModel = new IssueFormModel
            {
                Title = issue.Title,
                Description = issue.Description
            };

            return View(issueFormModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int _issueId, IssueFormModel _issueModel)
        {
            bool isEdited = await this.issueService.EditIssue(_issueId, _issueModel.Title, _issueModel.Description);

            //TO DO Checks...

            return RedirectToAction(nameof(MyIssues));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int _issueId)
        {
            bool isDeleted = await this.issueService.DeleteIssue(_issueId);

            //TO DO Checks...

            return RedirectToAction(nameof(MyIssues));
        }


        public async Task<IActionResult> Details(int _issueId)
        {
            var issueDetails = await this.issueService.GetIssueDetails(_issueId);

            return View(issueDetails);
        }


        //used only if the user role is "Admin"
        public async Task<IActionResult> AllIssues()
        {
            var allIssues = await this.issueService.GetAllReportedIssues();

            return View(allIssues);
        }


        public async Task<IActionResult> MyIssues()
        {
            var user = await this.userManagerService.GetUserAsync(HttpContext.User);
            var myIssues = await this.issueService.GetMyReportedIssues(user.Id);

            return View(myIssues);
        }

    }
}
