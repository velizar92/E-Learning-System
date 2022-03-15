namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Course;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;

    using static E_LearningSystem.Infrastructure.IdentityConstants;

    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly UserManager<User> userManagerService;


        public CoursesController(
            ICourseService courseService,
            IShoppingCartService shoppingCartService,
            UserManager<User> userManager)
        {
            this.courseService = courseService;
            this.shoppingCartService = shoppingCartService;
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
            string userId = User.Id();
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

            int courseId = await this.courseService.CreateCourse(
                                 userId,
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
            string shoppingCartId = null;
            var courses = await courseService.GetAllCourses();

            if (User.IsInRole(LearnerRole))
            {
                shoppingCartId = await this.shoppingCartService.GetCartIdByUserId(User.Id());
            }

            var allCoursesViewModel = new AllCoursesViewModel
            {
                ShoppingCartId = shoppingCartId,
                AllCoursesServiceModel = courses
            };

            return View(allCoursesViewModel);
        }


        [Authorize]
        public async Task<IActionResult> MyCourses()
        {
            string shoppingCartId = null;           
            var myCourses = await courseService.GetMyCourses(User.Id());

            if (User.IsInRole(LearnerRole))
            {
                shoppingCartId = await this.shoppingCartService.GetCartIdByUserId(User.Id());
            }

            var myCoursesViewModel = new AllCoursesViewModel
            {
                ShoppingCartId = shoppingCartId,
                AllCoursesServiceModel = myCourses
            };

            return View(myCoursesViewModel);
        }


        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            string shoppingCartId = null;
            var courseDetails = await courseService.GetCourseDetails(id);

            if (User.IsInRole(LearnerRole))
            {
                shoppingCartId = await this.shoppingCartService.GetCartIdByUserId(User.Id());
            }       

            var courseDetailsViewModel = new CourseDetailsViewModel
            {
                CourseServiceModel = courseDetails,
                ShoppingCartId = shoppingCartId
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
