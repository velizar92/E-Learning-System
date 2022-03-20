namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Issue;
    using E_LearningSystem.Infrastructure.Extensions;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;

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
        [Authorize(Roles = $"{AdminRole}, {LearnerRole}")]
        public IActionResult CreateIssue()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = $"{AdminRole}, {LearnerRole}")]
        public async Task<IActionResult> CreateIssue(int courseId, IssueFormModel issueModel)
        {
            string userId = User.Id();

            int issueId = await this.issueService.CreateIssue(userId, courseId, issueModel.Title, issueModel.Description);

            return RedirectToAction(nameof(MyReportedIssues));
        }


        [HttpGet]
        [Authorize(Roles = $"{AdminRole}, {LearnerRole}")]
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
        [Authorize(Roles = $"{AdminRole}, {LearnerRole}")]
        public async Task<IActionResult> EditIssue(int issueId, IssueFormModel issueModel)
        {
            bool isEdited = await this.issueService.EditIssue(issueId, issueModel.Title, issueModel.Description);

            if (isEdited == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(MyReportedIssues));
        }


        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
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


        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> MyIssues(int courseId)
        {
            var allIssues = await this.issueService.GetAllReportedIssuesForCourse(courseId);

            return View(allIssues);
        }


        [Authorize(Roles = $"{TrainerRole}, {LearnerRole}")]
        public async Task<IActionResult> MyReportedIssues()
        {
            string userId = User.Id();
            var myIssues = await this.issueService.GetMyReportedIssues(userId);

            return View(myIssues);
        }


        [Authorize(Roles = $"{TrainerRole}, {LearnerRole}")]
        public async Task<IActionResult> FixIssue(int issueId, int courseId)
        {
            bool isDeleted = await this.issueService.DeleteIssue(issueId);

            if (isDeleted == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(MyIssues));
        }

    }
}
