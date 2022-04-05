namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using Microsoft.AspNetCore.Authorization;
    using AspNetCoreHero.ToastNotification.Abstractions;
    using E_LearningSystem.Infrastructure.Extensions;
    using E_LearningSystem.Web.Models.Trainers;

    using static E_LearningSystem.Infrastructure.Messages.Messages;
    using static E_LearningSystem.Infrastructure.Messages.WarningMessages;
    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;
   
    public class TrainersController : Controller
    {

        private readonly ITrainerService trainerService;
        private readonly INotyfService notyfService;
        private readonly UserManager<User> userManager;

        public TrainersController(
            ITrainerService trainerService,
            INotyfService notyfService,
            UserManager<User> userManager)
        {
            this.trainerService = trainerService;
            this.notyfService = notyfService;
            this.userManager = userManager;
        }

        
        public async Task<IActionResult> AllTrainers()
        {
            var allActiveTrainers = await this.trainerService.GetActiveTrainers();

            return View(allActiveTrainers);
        }


        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> Vote(int id)
        {
            var isVoted = await this.trainerService.VoteForTrainer(User.Id(), id);

            if (isVoted == true)
            {
                this.notyfService.Success(SuccessfulVote);
            }
            else
            {
                this.notyfService.Warning(YouAlreadyVoteForThisTrainer);        
            }

            return RedirectToAction(nameof(AllTrainers));
        }


        [Authorize(Roles = LearnerRole)]
        public IActionResult BecomeTrainer()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = LearnerRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BecomeTrainer(BecomeTrainerFormModel formModel)
        {
            var userId = this.User.Id();
            var user = await userManager.GetUserAsync(HttpContext.User);

            var isUserAlreadyTrainer = await this.trainerService.CheckIfTrainerExists(userId);

            if (isUserAlreadyTrainer)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            await userManager.RemoveFromRoleAsync(user, LearnerRole);
            await userManager.AddToRoleAsync(user, TrainerRole);

            await this.trainerService.BecomeTrainer(user, formModel.FullName, formModel.CVUrl);
           
            return RedirectToAction(nameof(AllTrainers));
        }

    }
}
