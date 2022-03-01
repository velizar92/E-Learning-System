namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Lecture;

    public class LecturesController : Controller
    {
        private readonly ILectureService lectureService;
        private readonly UserManager<User> userManagerService;


        public LecturesController(ILectureService _lectureService, UserManager<User> _userManagerService)
        {
            this.lectureService = _lectureService;
            this.userManagerService = _userManagerService;
        }


        [HttpGet]
        public IActionResult CreateLecture()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateLecture(
            int _courseId,
            CreateLectureFormModel _lectureModel,
            IEnumerable<IFormFile> _resourceFiles)
        {
            int lectureId = await this.lectureService.AddLectureToCourse(
                                _courseId,
                                _lectureModel.Name,
                                _lectureModel.Description,
                                _resourceFiles);

            return RedirectToAction("Details", "Courses");
        }


        [HttpGet]
        public async Task<IActionResult> EditLecture(int _lectureId)
        {
            var lecture = await lectureService.GetLectureById(_lectureId);

            if(lecture == null)
            {
                return NotFound();
            }

            CreateLectureFormModel lectureFormModel = new CreateLectureFormModel()
            {
                Id = lecture.Id,
                Name = lecture.Name,
                Description = lecture.Description,
                Resources = lecture.Resources,
            };

            return View(lectureFormModel);

        }


        [HttpPost]
        public async Task<IActionResult> EditLecture(int _lectureId, CreateLectureFormModel _lectureModel)
        {
            var lecture = await lectureService.GetLectureById(_lectureId);

            if (lecture == null)
            {
                return NotFound();
            }
         
            lecture.Id = _lectureModel.Id;
            lecture.Name = _lectureModel.Name;
            lecture.Description = _lectureModel.Description;
            lecture.Resources = _lectureModel.Resources;

            return RedirectToAction(nameof(Details), new { _lectureId });
        }



        [HttpGet]
        public async Task<IActionResult> Details(int _lectureId)
        {
            var lectureDetails = await lectureService.GetLectureDetails(_lectureId);

            return View(lectureDetails);

        }


        [HttpPost]
        public async Task<IActionResult> Delete(int _lectureId)
        {
            var (isDeleted, courseId) = await lectureService.DeleteLecture(_lectureId);

            //TO DO Some check with "isDeleted"

            return RedirectToAction("Details", "Course", new { courseId });
        }

    }
}
