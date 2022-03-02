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
        private readonly ILectureService lectureService;
        private readonly UserManager<User> userManagerService;


        public ResourcesController(IResourceService resourceService, ILectureService lectureService, UserManager<User> userManagerService)
        {
            this.resourceService = resourceService;
            this.lectureService = lectureService;
            this.userManagerService = userManagerService;
        }

       
        public async Task<IActionResult> DeleteResource(int resourceId)
        {
            int id = await this.lectureService.GetLectureIdByResourceId(resourceId);
            bool isDeleted = await this.resourceService.DeleteResource(resourceId);
          
            return RedirectToAction("Details", "Lectures", new { id });
        }


        
        public async Task<IActionResult> MyResources()
        {
            string userId = User.Id();
            var myResources =  await this.resourceService.GetMyResources(userId);

            return View(myResources);
        }


    }
}
