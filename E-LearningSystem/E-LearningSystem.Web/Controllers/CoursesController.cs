namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Course;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;
    using E_LearningSystem.Services.Services.Courses.Models;

    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly ITrainerService trainerService;
        private readonly UserManager<User> userManagerService;


        public CoursesController(
            ICourseService courseService,
            IShoppingCartService shoppingCartService,
            ITrainerService trainerService,
            UserManager<User> userManager)
        {
            this.courseService = courseService;
            this.shoppingCartService = shoppingCartService;
            this.trainerService = trainerService;
            this.userManagerService = userManager;
        }


        [HttpGet]
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> CreateCourse()
        {
            return View(new CourseFormModel
            {
                Categories = await this.courseService.GetAllCourseCategories()
            });
        }


        [HttpPost]
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> CreateCourse(CourseFormModel courseModel, IFormFile pictureFile)
        {                 
            ModelState.Remove("Categories");

            if (!this.courseService.CheckIfCourseCategoryExists(courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(courseModel.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                courseModel.Categories = await this.courseService.GetAllCourseCategories();

                return View(courseModel);
            }

            int trainerId = await this.trainerService.GetTrainerIdByUserId(User.Id());

            int courseId = await this.courseService.CreateCourse(
                                 User.Id(),
                                 trainerId,
                                 courseModel.Name,
                                 courseModel.Description,
                                 courseModel.Price,
                                 courseModel.CategoryId,
                                 pictureFile);


            return RedirectToAction(nameof(MyCourses));
        }


        [HttpGet]      
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> EditCourse(int id, IFormFile pictureFile)
        {
            var course = await this.courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound();
            }

            CourseFormModel courseFormModel = new CourseFormModel
            {
                Name = course.Name,
                Description = course.Description,
                CategoryId = course.CategoryId,
            };

            return View(courseFormModel);
        }


        [HttpPost]       
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> EditCourse(int id, CourseFormModel courseModel, IFormFile pictureFile)
        {
            if (!this.courseService.CheckIfCourseCategoryExists(courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(courseModel.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                courseModel.Categories = await courseService.GetAllCourseCategories();

                return View(courseModel);
            }

            var course = courseService.EditCourse(
                                id,
                                courseModel.Name,
                                courseModel.Description,
                                courseModel.CategoryId,
                                pictureFile);

            return RedirectToAction(nameof(MyCourses));
        }



        public async Task<IActionResult> AllCourses()
        {         
            var courses = await courseService.GetAllCourses();
       
            var allCoursesViewModel = new AllCoursesViewModel
            {           
                AllCoursesServiceModel = courses
            };

            return View(allCoursesViewModel);
        }


        [Authorize]
        public async Task<IActionResult> MyCourses()
        {           
            IEnumerable<AllCoursesServiceModel> myCourses = null;

            if (User.IsInRole(TrainerRole))
            {
                int trainerId = await this.trainerService.GetTrainerIdByUserId(User.Id());
                myCourses = await courseService.GetMyCourses(trainerId);
            }
            else
            {
                myCourses = await courseService.GetMyCourses(User.Id());
            }
      
            var myCoursesViewModel = new AllCoursesViewModel
            {              
                AllCoursesServiceModel = myCourses
            };

            return View(myCoursesViewModel);
        }


        [Authorize]
        public async Task<IActionResult> Details(int id)
        {         
            var courseDetails = await courseService.GetCourseDetails(id);
                 
            var courseDetailsViewModel = new CourseDetailsViewModel
            {
                CourseServiceModel = courseDetails,              
            };

            return View(courseDetailsViewModel);
        }


        [Authorize]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await courseService.DeleteCourse(id);

            if (result == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(AllCourses));
        }
    }
}
