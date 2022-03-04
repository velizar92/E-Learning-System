namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Infrastructure.Extensions;

    using static E_LearningSystem.Infrastructure.IdentityConstants;
    using Microsoft.AspNetCore.Authorization;

    public class ResourcesController : Controller
    {
        private readonly IResourceService resourceService;
        private readonly ILectureService lectureService;
       


        public ResourcesController(IResourceService resourceService, ILectureService lectureService)
        {
            this.resourceService = resourceService;
            this.lectureService = lectureService;          
        }


        [Authorize(Roles = $"{AdminRole}, {TrainerRole}")]
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
