namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using AspNetCoreHero.ToastNotification.Abstractions;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Course;
    using E_LearningSystem.Infrastructure.Extensions;
    using E_LearningSystem.Services.Services.Courses.Models;
    using E_LearningSystem.Services.Services.Users;
    using E_LearningSystem.Services.Services.Storage;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;
    using static E_LearningSystem.Infrastructure.Messages.ErrorMessages;


    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly INotyfService notyfService;
        private readonly ITrainerService trainerService;
        private readonly IUserService userService;
        private readonly IStorageService storageService;
        private readonly UserManager<User> userManager;


        public CoursesController(
            ICourseService courseService,
            IShoppingCartService shoppingCartService,
            INotyfService notyfService,
            IUserService userService,
            IStorageService storageService,
            ITrainerService trainerService,

            UserManager<User> userManager)
        {
            this.courseService = courseService;
            this.shoppingCartService = shoppingCartService;
            this.notyfService = notyfService;
            this.userService = userService;
            this.storageService = storageService;
            this.trainerService = trainerService;
            this.userManager = userManager;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(CourseFormModel courseModel)
        {
            if (!this.courseService.CheckIfCourseCategoryExists(courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(courseModel.CategoryId), CategoryNotExists);
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
                                 courseModel.PictureFile.FileName);

            await this.storageService.SaveFile(@"\assets\img\courses", courseModel.PictureFile);

            return RedirectToAction(nameof(MyCourses));
        }


        [HttpGet]
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> EditCourse(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            int courseCreatorId = await this.courseService.GetCourseCreatorId(id);
            int currentTrainerId = await this.trainerService.GetTrainerIdByUserId(User.Id());

            if (currentTrainerId == courseCreatorId || await userManager.IsInRoleAsync(user, AdminRole))
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
                    Price = course.Price,
                    CategoryId = course.CategoryId,
                    Categories = await courseService.GetAllCourseCategories()
                };

                return View(courseFormModel);
            }

            return Unauthorized();
        }


        [HttpPost]
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, CourseFormModel courseModel)
        {
            if (!this.courseService.CheckIfCourseCategoryExists(courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(courseModel.CategoryId), CategoryNotExists);
            }

            if (!ModelState.IsValid)
            {
                courseModel.Categories = await courseService.GetAllCourseCategories();

                return View(courseModel);
            }

            var isEdited = await courseService.EditCourse(
                                id,
                                courseModel.Name,
                                courseModel.Description,
                                courseModel.Price,
                                courseModel.CategoryId,
                                courseModel.PictureFile.FileName);
            
            if (isEdited == false)
            {
                return BadRequest();
            }

            await this.storageService.SaveFile(@"\assets\img\courses", courseModel.PictureFile);

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
            IEnumerable<AllCoursesServiceModel> myCourses = new List<AllCoursesServiceModel>();

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
            bool hasCourse = false;

            hasCourse = await this.userService.CheckIfUserHasCourse(User.Id(), id);

            var courseDetailsViewModel = new CourseDetailsViewModel
            {
                Id = courseDetails.Id,
                Name = courseDetails.Name,
                Description = courseDetails.Description,
                ImageUrl = courseDetails.ImageUrl,
                AssignedStudents = courseDetails.AssignedStudents,
                Price = courseDetails.Price,
                Trainer = courseDetails.Trainer,
                HasCourse = hasCourse,
                Lectures = courseDetails.Lectures,
            };

            return View(courseDetailsViewModel);
        }


        [Authorize]
        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            int courseCreatorId = await this.courseService.GetCourseCreatorId(id);
            int currentTrainerId = await this.trainerService.GetTrainerIdByUserId(User.Id());

            if (currentTrainerId == courseCreatorId || await userManager.IsInRoleAsync(user, AdminRole))
            {
                var result = await courseService.DeleteCourse(id);

                if (result == false)
                {
                    return BadRequest();
                }

                return RedirectToAction(nameof(AllCourses));
            }

            return Unauthorized();
        }
    }
}
