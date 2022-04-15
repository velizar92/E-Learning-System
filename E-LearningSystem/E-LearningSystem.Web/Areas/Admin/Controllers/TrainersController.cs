namespace E_LearningSystem.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using System.Security.Claims;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Areas.Admin.Models;
    using E_LearningSystem.Services.Services.Storage;
    using E_LearningSystem.Data.Models;
   

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;

    public class TrainersController : AdminController
    {
        private readonly ITrainerService trainerService;
        private readonly IStorageService storageService;
        private readonly UserManager<User> userManager;

        public TrainersController(
            ITrainerService trainerService,
            UserManager<User> userManager,
            IStorageService storageService)
        {
            this.trainerService = trainerService;
            this.userManager = userManager;
            this.storageService = storageService;   
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

            var user = GetUser(
                                formModel.FirstName,
                                formModel.LastName,
                                formModel.Email,
                                formModel.Password,
                                formModel.ProfileImage.FileName,
                                formModel.CV.FileName);

            await userManager.CreateAsync(user, formModel.Password);
            await userManager.AddToRoleAsync(user, TrainerRole);
            await userManager.AddClaimAsync(user, new Claim("ProfileImageUrl", user.ProfileImageUrl));

            await this.trainerService.CreateTrainer(
                                            user,
                                            formModel.FirstName,
                                            formModel.LastName,
                                            formModel.Email,
                                            formModel.Password,
                                            formModel.ProfileImage.FileName,
                                            formModel.CV.FileName);

            await this.storageService.SaveFile(@"\assets\img\users", formModel.ProfileImage);
            await this.storageService.SaveFile(@"\assets\resources", formModel.CV);

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


        private User GetUser(string firstName, string lastName,
            string email, string password, string profileImageUrl, string cvUrl)
        {
            User user = new()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                Email = email,
                ProfileImageUrl = profileImageUrl,
            };

            return user;
        }

    }
}
