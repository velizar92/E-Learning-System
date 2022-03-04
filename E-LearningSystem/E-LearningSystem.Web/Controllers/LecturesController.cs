namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Lecture;

    using static E_LearningSystem.Infrastructure.IdentityConstants;
  
    public class LecturesController : Controller
    {
        private readonly ILectureService lectureService;
        private readonly UserManager<User> userManagerService;


        public LecturesController(ILectureService lectureService, UserManager<User> userManagerService)
        {
            this.lectureService = lectureService;
            this.userManagerService = userManagerService;
        }


        [HttpGet]
        public IActionResult CreateLecture()
        {
            return View();
        }


        [HttpPost, DisableRequestSizeLimit]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> CreateLecture(
            int id,
            CreateLectureFormModel lectureModel,
            IEnumerable<IFormFile> files)
        {
            int lectureId = await this.lectureService.AddLectureToCourse(
                                id,
                                lectureModel.Name,
                                lectureModel.Description,
                                files);

            return RedirectToAction("Details", "Courses", new { id });
        }


        [HttpGet]
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> EditLecture(int id)
        {
            var lecture = await lectureService.GetLectureById(id);

            if(lecture == null)
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


        [HttpPost]
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> EditLecture(int id, CreateLectureFormModel lectureModel, IEnumerable<IFormFile> files)
        {
            var lecture = await lectureService.GetLectureById(id);

            if (lecture == null)
            {
                return NotFound();
            }

            bool isEdited = await this.lectureService.EditLecture(id, lectureModel.Name, lectureModel.Description, files);

            return RedirectToAction(nameof(Details), new { id });
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var lectureDetails = await lectureService.GetLectureDetails(id);

            return View(lectureDetails);

        }


        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            var (isDeleted, courseId) = await lectureService.DeleteLecture(id);

            //TO DO Some check with "isDeleted"

            id = courseId;

            return RedirectToAction("Details", "Courses", new { id });
        }

    }
}
