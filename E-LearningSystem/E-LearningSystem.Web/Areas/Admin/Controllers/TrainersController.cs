namespace E_LearningSystem.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Services.Services;

    public class TrainersController : AdminController
    {
        private readonly ITrainerService trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }

        public async Task<IActionResult> Index()
        {
            var allTrainers = await this.trainerService.GetAllTrainers();

            return View(allTrainers);          
        }
    }
}
