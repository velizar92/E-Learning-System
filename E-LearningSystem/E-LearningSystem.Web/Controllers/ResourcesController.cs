namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Infrastructure.Extensions;

    public class ResourcesController : Controller
    {
        private readonly IResourceService resourceService;
        private readonly UserManager<User> userManagerService;


        public ResourcesController(IResourceService _resourceService, UserManager<User> _userManagerService)
        {
            this.resourceService = _resourceService;
            this.userManagerService = _userManagerService;
        }


        [HttpPost]
        public async Task<IActionResult> DeleteResource(int _resourceId)
        {
            bool isDeleted = await this.resourceService.DeleteResource(_resourceId);

            return RedirectToAction(nameof(MyResources));
        }


        
        public async Task<IActionResult> MyResources()
        {
            string userId = User.Id();
            var myResources =  await this.resourceService.GetMyResources(userId);

            return View(myResources);
        }


    }
}
