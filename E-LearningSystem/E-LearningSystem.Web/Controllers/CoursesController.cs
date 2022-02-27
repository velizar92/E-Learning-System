namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Services.Services;
    
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService _courseService)
        {
            this.courseService = _courseService;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> All()
        {
            var courses = await courseService.GetAllCourses();

            return View(courses);
        }


        public async Task<IActionResult> Details(int id)
        {
            var courseDetails = await courseService.GetCourseDetails(id);

            return View(courseDetails);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await courseService.DeleteCourse(id);

            if(result == false)
            {
                return BadRequest();
            }

            return RedirectToAction("All");
        }
    }
}
