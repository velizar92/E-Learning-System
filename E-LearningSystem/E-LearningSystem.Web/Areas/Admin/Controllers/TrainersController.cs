namespace E_LearningSystem.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Areas.Admin.Models;

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

        [HttpGet]
        public IActionResult CreateTrainer()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainer(TrainerFormModel formModel)
        {
            await this.trainerService.CreateTrainer(
                                            formModel.FirstName,
                                            formModel.LastName,
                                            formModel.Email,
                                            formModel.Password,
                                            formModel.ProfileImage,
                                            formModel.CV);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ApproveTrainer(int id)
        {
            bool isApproved = await this.trainerService.ApproveTrainer(id);

            if (isApproved == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeleteTrainer(int id, string userId)
        {
            bool isDeleted = await this.trainerService.DeleteTrainer(id, userId);

            if (isDeleted == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
