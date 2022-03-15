namespace E_LearningSystem.Web.Controllers
{
    using System.Diagnostics;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Web.Models.Home;
    using E_LearningSystem.Infrastructure.Extensions;

    using static E_LearningSystem.Infrastructure.IdentityConstants;
  
    public class HomeController : Controller
    {
        private readonly ITrainerService trainerService;
        private readonly ICourseService courseService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly UserManager<User> userManagerService;

        public HomeController(
            ITrainerService trainerService,
            ICourseService courseService,
            IShoppingCartService shoppingCartService,
            UserManager<User> userManagerService)
        {
            this.trainerService = trainerService;
            this.courseService = courseService;
            this.shoppingCartService = shoppingCartService;
            this.userManagerService = userManagerService;
        }


        public async Task<IActionResult> Index()
        {
            string shoppingCartId = null;
            var allLearners = await this.userManagerService.GetUsersInRoleAsync(LearnerRole);
            var allTrainers = await this.trainerService.GetAllTrainers();
            var topTrainers = await this.trainerService.GetTopTrainers();
            var latestCourses = await this.courseService.GetLatestCourses(3);
            var allCourses = await this.courseService.GetAllCourses();

            if (User.IsInRole(LearnerRole))
            {
                shoppingCartId = await this.shoppingCartService.GetCartIdByUserId(User.Id());
            }

            var indexViewModel = new IndexViewModel
            {
                ShoppingCartId = shoppingCartId,
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
            string shoppingCartId = null;
            var allLearners = await this.userManagerService.GetUsersInRoleAsync(LearnerRole);
            var allTrainers = await this.trainerService.GetAllTrainers();
            var allCourses = await this.courseService.GetAllCourses();

            if (User.IsInRole(LearnerRole))
            {
                shoppingCartId = await this.shoppingCartService.GetCartIdByUserId(User.Id());
            }

            var aboutViewModel = new AboutViewModel
            {
                ShoppingCartId = shoppingCartId,
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