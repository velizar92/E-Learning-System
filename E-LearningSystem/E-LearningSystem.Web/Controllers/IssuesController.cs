namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Issue;
    using E_LearningSystem.Services.Services.Users;
    using E_LearningSystem.Infrastructure.Extensions;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;
    

    public class IssuesController : Controller
    {
        private readonly IIssueService issueService;
        private readonly IUserService userService;
        private readonly UserManager<User> userManagerService;


        public IssuesController(
            IIssueService issueService,
            IUserService userService,
            UserManager<User> userManagerService)
        {
            this.issueService = issueService;
            this.userService = userService;
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
        public async Task<IActionResult> CreateIssue(IssueFormModel issueModel, int id)
        {
            string userId = User.Id();

            int issueId = await this.issueService.CreateIssue(userId, id, issueModel.Title, issueModel.Description);

            return RedirectToAction(nameof(MyReportedIssues), new { id });
        }


        [HttpGet]
        [Authorize(Roles = $"{AdminRole}, {LearnerRole}")]
        public async Task<IActionResult> EditIssue(int id)
        {
            var issue = await this.issueService.GetIssueDetails(id);

            IssueFormModel issueFormModel = new IssueFormModel
            {
                CourseId = issue.CourseId,
                Title = issue.Title,
                Description = issue.Description
            };

            return View(issueFormModel);
        }


        [HttpPost]
        [Authorize(Roles = $"{AdminRole}, {LearnerRole}")]
        public async Task<IActionResult> EditIssue(int id, IssueFormModel issueModel)
        {
            bool isEdited = await this.issueService.EditIssue(id, issueModel.Title, issueModel.Description);

            if (isEdited == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(MyReportedIssues), new { id });
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


        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> MyReportedIssues(int id)
        {
            string userId = User.Id();
            var myIssues = await this.issueService.GetMyReportedIssues(userId, id);

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

            return RedirectToAction(nameof(MyIssues), new { courseId });
        }

    }
}
