namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using AspNetCoreHero.ToastNotification.Abstractions;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Lecture;
    using E_LearningSystem.Services.Services.Users;
    using E_LearningSystem.Infrastructure.Extensions;
    using E_LearningSystem.Services.Services.Storage;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;
    using static E_LearningSystem.Infrastructure.Constants.MimeTypeConstants;

    public class LecturesController : Controller
    {
        private readonly ILectureService lectureService;
        private readonly IUserService userService;
        private readonly INotyfService notyfService;
        private readonly IStorageService storageService;
        private readonly UserManager<User> userManager;


        public LecturesController(
            ILectureService lectureService,
            IUserService userService,
            INotyfService notyfService,
            IStorageService storageService,
            UserManager<User> userManagerService)
        {
            this.lectureService = lectureService;
            this.userService = userService;
            this.notyfService = notyfService;
            this.storageService = storageService;
            this.userManager = userManagerService;
        }


        [HttpGet]
        [Authorize]
        public IActionResult CreateLecture()
        {
            return View();
        }


        [HttpPost, DisableRequestSizeLimit]       
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLecture(
            int id,
            CreateLectureFormModel lectureModel)
        {    
            if (!ModelState.IsValid)
            {
                return View(lectureModel);
            }

            var resources = GetResources(lectureModel.Files);

            int lectureId = await this.lectureService.AddLectureToCourse(
                                id,
                                lectureModel.Name,
                                lectureModel.Description,
                                resources);

            if (lectureId == -1)
            {
                return BadRequest();
            }

            await storageService.SaveFiles(@"\assets\resources", lectureModel.Files);
            return RedirectToAction("Details", "Courses", new { id });
        }


        [HttpGet]
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> EditLecture(int id)
        {
            var lecture = await lectureService.GetLectureById(id);

            if (lecture == null)
            {
                return NotFound();
            }

            CreateLectureFormModel lectureFormModel = new CreateLectureFormModel()
            {
                Id = lecture.Id,
                CourseId = lecture.CourseId,
                Name = lecture.Name,
                Description = lecture.Description,
                Resources = lecture.Resources,
            };

            return View(lectureFormModel);

        }


        [HttpPost, DisableRequestSizeLimit]   
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLecture(int id, CreateLectureFormModel lectureModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(lectureModel);
            }

            var lecture = await lectureService.GetLectureById(id);

            if (lecture == null)
            {
                return NotFound();
            }

            var resources = GetResources(lectureModel.Files);
            bool isEdited = await this.lectureService.EditLecture(id, lectureModel.Name, lectureModel.Description, resources);

            if (isEdited == false)
            {
                return BadRequest();
            }

            await storageService.SaveFiles(@"\assets\resources", lectureModel.Files);
            return RedirectToAction(nameof(Details), new { id });
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var lectureDetails = await lectureService.GetLectureDetails(id);

            var isBuyed = await this.userService.CheckIfUserHasCourse(User.Id(), lectureDetails.CourseId);

            if (isBuyed == true || await userManager.IsInRoleAsync(user, AdminRole))
            {
                return View(lectureDetails);
            }
            else
            {
                id = lectureDetails.CourseId;
                TempData["AlertMessage"] = "You don't have an access to this course.";
                return RedirectToAction("Details", "Courses", new { id });
            }
        }


        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            var (isDeleted, courseId) = await lectureService.DeleteLecture(id);

            if (isDeleted == true)
            {
                id = courseId;

                return RedirectToAction("Details", "Courses", new { id });
            }

            return BadRequest();
        }

        private List<Resource> GetResources(IEnumerable<IFormFile> resourceFiles)
        {
            List<Resource> resources = new List<Resource>();
            int tempResourceId = 1;

            foreach (var resourceFile in resourceFiles)
            {
                if (resourceFile.ContentType == PDF)
                    tempResourceId = 3;
                else if (resourceFile.ContentType == MP4)
                    tempResourceId = 2;
                else
                    tempResourceId = 1;

                resources.Add(new Resource { Name = resourceFile.FileName, ResourceTypeId = tempResourceId });
            }

            return resources;
        }

    }
}
