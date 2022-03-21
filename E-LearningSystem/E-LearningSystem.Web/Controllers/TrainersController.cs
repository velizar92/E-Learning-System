namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Services.Services;
    using Microsoft.AspNetCore.Authorization;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;

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


        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> Vote(int id)
        {
            var isVoted = await this.trainerService.VoteForTrainer(id);

            if (isVoted == true)
            {
                return RedirectToAction(nameof(AllTrainers));
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
