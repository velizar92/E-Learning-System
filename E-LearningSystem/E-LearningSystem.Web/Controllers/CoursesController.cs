namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Course;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using E_LearningSystem.Services.Services.Courses.Models;
    using AspNetCoreHero.ToastNotification.Abstractions;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;

    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly ITrainerService trainerService;
        private readonly INotyfService notyfService;
        private readonly UserManager<User> userManagerService;


        public CoursesController(
            ICourseService courseService,
            IShoppingCartService shoppingCartService,
            INotyfService notyfService,
            ITrainerService trainerService,

            UserManager<User> userManager)
        {
            this.courseService = courseService;
            this.shoppingCartService = shoppingCartService;
            this.notyfService = notyfService;
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
            int courseCreatorId = await this.courseService.GetCourseCreatorId(id);
            int currentTrainerId = await this.trainerService.GetTrainerIdByUserId(User.Id());

            if (currentTrainerId != courseCreatorId)
            {
                return Unauthorized();
            }

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
                Categories = await courseService.GetAllCourseCategories()
            };

            bool isEdited = await this.courseService.EditCourse(
                id,
                courseFormModel.Name,
                courseFormModel.Description,
                courseFormModel.CategoryId,
                pictureFile);

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
            List<AllCoursesViewModel> allCoursesViewModel = new List<AllCoursesViewModel>();
            var courses = await courseService.GetAllCourses();

            foreach (var course in courses)
            {
                var trainer = await this.trainerService.GetTrainerByTrainerId(course.TrainerId);

                var viewModel = new AllCoursesViewModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,   
                    Price = course.Price,
                    ImageUrl = course.ImageUrl,
                    CategoryName = course.CategoryName,
                    AssignedStudents = course.AssignedStudents,
                    ProfileImageUrl = trainer.ProfileImageUrl,
                    TrainerName = trainer.FullName
                };

                allCoursesViewModel.Add(viewModel);
            }
        
            return View(allCoursesViewModel);
        }


        [Authorize]
        public async Task<IActionResult> MyCourses()
        {
            List<AllCoursesViewModel> myCoursesViewModel = new List<AllCoursesViewModel>();
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

            foreach (var course in myCourses)
            {
                var trainer = await this.trainerService.GetTrainerByTrainerId(course.TrainerId);

                var viewModel = new AllCoursesViewModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    Price = course.Price,
                    ImageUrl = course.ImageUrl,
                    CategoryName = course.CategoryName,
                    AssignedStudents = course.AssignedStudents,
                    ProfileImageUrl = trainer.ProfileImageUrl,
                    TrainerName = trainer.FullName
                };

                myCoursesViewModel.Add(viewModel);
            }

            return View(myCoursesViewModel);
        }


        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var courseDetails = await courseService.GetCourseDetails(id);

            return View(courseDetails);
        }


        [Authorize]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            int courseCreatorId = await this.courseService.GetCourseCreatorId(id);
            int currentTrainerId = await this.trainerService.GetTrainerIdByUserId(User.Id());

            if (currentTrainerId != courseCreatorId)
            {
                return Unauthorized();
            }

            var result = await courseService.DeleteCourse(id);

            if (result == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(AllCourses));
        }
    }
}
