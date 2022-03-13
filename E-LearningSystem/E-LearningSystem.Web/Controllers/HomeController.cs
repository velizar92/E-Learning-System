namespace E_LearningSystem.Web.Controllers
{
    using System.Diagnostics;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models;
    using E_LearningSystem.Web.Models.Index;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using static E_LearningSystem.Data.DataConstants;
    using static E_LearningSystem.Infrastructure.IdentityConstants;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ITrainerService trainerService;
        private readonly ICourseService courseService;
        private readonly UserManager<User> userManagerService;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<User> userManagerService,
            ITrainerService trainerService,
            ICourseService courseService)
        {
            this.logger = logger;
            this.userManagerService = userManagerService;
            this.trainerService = trainerService;
            this.courseService = courseService;
        }


        public IActionResult Index()
        {
            var allLearners = this.userManagerService.GetUsersInRoleAsync(LearnerRole);
            var allTrainers = this.trainerService.GetAllTrainers();
            var topTrainers = this.trainerService.GetTopTrainers();
            var latestCourses = this.courseService.GetLatestCourses(3);
            var allCourses = this.courseService.GetAllCourses();

            var indexViewModel = new IndexViewModel
            {
                CoursesCount = allCourses.Result.Count(),
                TrainersCount = allTrainers.Result.Count(),
                LearnersCount = allLearners.Result.Count(),
                LatestCourses = latestCourses.Result,
                TopTrainers = topTrainers.Result
            };

            return View(indexViewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}