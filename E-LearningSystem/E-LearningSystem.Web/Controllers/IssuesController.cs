namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Issue;
    using E_LearningSystem.Infrastructure.Extensions;

    using static E_LearningSystem.Infrastructure.IdentityConstants;
    

    public class IssuesController : Controller
    {
        private readonly IIssueService issueService;
        private readonly UserManager<User> userManagerService;


        public IssuesController(IIssueService issueService, UserManager<User> userManagerService)
        {
            this.issueService = issueService;
            this.userManagerService = userManagerService;
        }


        [HttpGet]
        [Authorize(Roles = LearnerRole)]
        [Authorize(Roles = AdminRole)]
        public IActionResult CreateIssue()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = LearnerRole)]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> CreateIssue(int id, IssueFormModel issueModel)
        {
            string userId = User.Id();

            int issueId = await this.issueService.CreateIssue(userId, id, issueModel.Title, issueModel.Description);

            return RedirectToAction(nameof(MyIssues));
        }


        [HttpGet]
        [Authorize(Roles = LearnerRole)]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> EditIssue(int issueId)
        {
            var issue = await this.issueService.GetIssueDetails(issueId);

            IssueFormModel issueFormModel = new IssueFormModel
            {
                Title = issue.Title,
                Description = issue.Description
            };

            return View(issueFormModel);
        }


        [HttpPost]
        [Authorize(Roles = LearnerRole)]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> EditIssue(int issueId, IssueFormModel issueModel)
        {
            bool isEdited = await this.issueService.EditIssue(issueId, issueModel.Title, issueModel.Description);

            if (isEdited == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(MyIssues));
        }


        public async Task<IActionResult> DeleteIssue(int issueId)
        {
            bool isDeleted = await this.issueService.DeleteIssue(issueId);

            if (isDeleted == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(MyIssues));
        }


        public async Task<IActionResult> Details(int issueId)
        {
            var issueDetails = await this.issueService.GetIssueDetails(issueId);

            return View(issueDetails);
        }


        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> AllIssues()
        {
            var allIssues = await this.issueService.GetAllReportedIssues();

            return View(allIssues);
        }


        [Authorize(Roles = TrainerRole)]
        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> MyIssues()
        {
            string userId = User.Id();
            var myIssues = await this.issueService.GetMyReportedIssues(userId);

            return View(myIssues);
        }

    }
}
