namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Course;
    using E_LearningSystem.Data.Models;


    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly UserManager<User> userManagerService;

        public CoursesController(ICourseService _courseService, UserManager<User> _userManager)
        {
            this.courseService = _courseService;
            this.userManagerService = _userManager;
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new CourseFormModel
            {
                Categories = await this.courseService.GetAllCourseCategories()
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create(CourseFormModel _courseModel, IFormFile _pictureFile)
        {
            var user = await this.userManagerService.GetUserAsync(HttpContext.User);

            if (!this.courseService.CheckIfCourseCategoryExists(_courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(_courseModel.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                _courseModel.Categories = await this.courseService.GetAllCourseCategories();

                return View(_courseModel);
            }

            int courseId = await this.courseService.CreateCourse(
                                 user.Id,
                                 _courseModel.Name,
                                 _courseModel.Description,
                                 _courseModel.CategoryId,
                                 _pictureFile);


            return RedirectToAction(nameof(MyCourses));
        }


        [HttpGet]
        public IActionResult Edit(int _id)
        {
            var course = this.courseService.GetCourseById(_id);

            return View(course);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int _courseId, CourseFormModel _courseModel, IFormFile _pictureFile)
        {
            var user = await userManagerService.GetUserAsync(HttpContext.User);

            if (!this.courseService.CheckIfCourseCategoryExists(_courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(_courseModel.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                _courseModel.Categories = await courseService.GetAllCourseCategories();

                return View(_courseModel);
            }

            var course = courseService.EditCourse(
                                _courseId,
                                _courseModel.Name,
                                _courseModel.Description,
                                _courseModel.CategoryId,
                                _pictureFile);


            return RedirectToAction(nameof(MyCourses));
        }



        public async Task<IActionResult> AllCourses()
        {
            var courses = await courseService.GetAllCourses();

            return View(courses);
        }



        public async Task<IActionResult> MyCourses()
        {
            var user = await userManagerService.GetUserAsync(HttpContext.User);
            var myCourses = await courseService.GetMyCourses(user.Id);

            return View(myCourses);
        }


        public async Task<IActionResult> Details(int _id)
        {
            var courseDetails = await courseService.GetCourseDetails(_id);

            return View(courseDetails);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int _id)
        {
            var result = await courseService.DeleteCourse(_id);

            if (result == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(AllCourses));
        }
    }
}
