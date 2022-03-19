namespace E_LearningSystem.Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using AspNetCoreHero.ToastNotification.Abstractions;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Web.Models.Home;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;
    using static E_LearningSystem.Infrastructure.Constants.MessageConstants;
   

    public class HomeController : Controller
    {
        private readonly ITrainerService trainerService;
        private readonly ICourseService courseService;
        private readonly INotyfService notyfService;     
        private readonly UserManager<User> userManagerService;

        public HomeController(
            ITrainerService trainerService,
            ICourseService courseService,
            INotyfService notyfService,          
            UserManager<User> userManagerService)
        {
            this.trainerService = trainerService;
            this.courseService = courseService;
            this.notyfService = notyfService;         
            this.userManagerService = userManagerService;
        }


        public async Task<IActionResult> Index()
        {
            var allLearners = await this.userManagerService.GetUsersInRoleAsync(LearnerRole);
            var allTrainers = await this.trainerService.GetAllTrainers();
            var topTrainers = await this.trainerService.GetTopTrainers();
            var latestCourses = await this.courseService.GetLatestCourses(3);
            var allCourses = await this.courseService.GetAllCourses();

            var indexViewModel = new IndexViewModel
            {              
                CoursesCount = allCourses.Count(),
                TrainersCount = allTrainers.Count(),
                LearnersCount = allLearners.Count(),
                LatestCourses = latestCourses,
                TopTrainers = topTrainers,
            };

            return View(indexViewModel);
        }


        public async Task<IActionResult> About()
        {
           
            var allLearners = await this.userManagerService.GetUsersInRoleAsync(LearnerRole);
            var allTrainers = await this.trainerService.GetAllTrainers();
            var allCourses = await this.courseService.GetAllCourses();

            var aboutViewModel = new AboutViewModel
            {            
                CoursesCount = allCourses.Count(),
                TrainersCount = allTrainers.Count(),
                LearnersCount = allLearners.Count(),
            };

            return View(aboutViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}