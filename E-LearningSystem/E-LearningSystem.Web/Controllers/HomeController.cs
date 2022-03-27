namespace E_LearningSystem.Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Caching.Memory;
    using AspNetCoreHero.ToastNotification.Abstractions;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Web.Models.Home;
    using E_LearningSystem.Services.Services.Courses.Models;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;
    using static E_LearningSystem.Infrastructure.Constants.CacheConstants;
    
    public class HomeController : Controller
    {
        private readonly ITrainerService trainerService;
        private readonly ICourseService courseService;
        private readonly INotyfService notyfService;
        private readonly IMemoryCache cache;
        private readonly UserManager<User> userManagerService;

        public HomeController(
            ITrainerService trainerService,
            ICourseService courseService,
            INotyfService notyfService,
            IMemoryCache cache,
            UserManager<User> userManagerService)
        {
            this.trainerService = trainerService;
            this.courseService = courseService;
            this.notyfService = notyfService;
            this.cache = cache;
            this.userManagerService = userManagerService;
        }


        public async Task<IActionResult> Index()
        {
            var IndexViewModel = await this.GetIndexViewModel();

            return View(IndexViewModel);
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


        private async Task<IndexViewModel> GetIndexViewModel()
        {
            var latestCourses = this.cache.Get<IEnumerable<LatestCoursesServiceModel>>(LatestCoursesCacheKey);
            var allCourses = this.cache.Get<IEnumerable<AllCoursesServiceModel>>(AllCoursesCacheKey);
            var allTrainers = this.cache.Get<IEnumerable<AllTrainersServiceModel>>(AllTrainersCacheKey);
            var topTrainers = this.cache.Get<IEnumerable<AllTrainersServiceModel>>(TopTrainersCacheKey);

            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

            if (latestCourses == null)
            {
                latestCourses = await this.courseService.GetLatestCourses(3);
                this.cache.Set(LatestCoursesCacheKey, latestCourses, cacheOptions);
            }

            if (allCourses == null)
            {
                allCourses = await this.courseService.GetAllCourses();
                this.cache.Set(AllCoursesCacheKey, allCourses, cacheOptions);
            }

            if (allTrainers == null)
            {
                allTrainers = await this.trainerService.GetAllTrainers();
                this.cache.Set(AllTrainersCacheKey, allTrainers, cacheOptions);
            }

            if (topTrainers == null)
            {
                topTrainers = await this.trainerService.GetTopTrainers();
                this.cache.Set(TopTrainersCacheKey, topTrainers, cacheOptions);
            }

            var allLearners = await this.userManagerService.GetUsersInRoleAsync(LearnerRole);

            var indexViewModel = new IndexViewModel
            {
                CoursesCount = allCourses.Count(),
                TrainersCount = allTrainers.Count(),
                LearnersCount = allLearners.Count(),
                LatestCourses = latestCourses,
                TopTrainers = topTrainers,
            };

            return indexViewModel;
        }
    }
}