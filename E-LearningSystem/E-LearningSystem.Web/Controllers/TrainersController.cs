namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Services.Services;

    public class TrainersController : Controller
    {

        private readonly ITrainerService trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }

        
        public async Task<IActionResult> AllTrainers()
        {
            var allTrainers = await this.trainerService.GetAllTrainers();

            return View(allTrainers);
        }
      
    }
}
