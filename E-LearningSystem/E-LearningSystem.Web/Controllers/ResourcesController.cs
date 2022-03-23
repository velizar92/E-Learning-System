namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using E_LearningSystem.Web.Models.Resource;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Infrastructure.Extensions;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;
   

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

      
        public async Task<IActionResult> MyResources([FromQuery] AllResourcesQueryModel query)
        {          
            string userId = User.Id();

            var myResourceServiceModel = await this.resourceService.GetAllMyResources(
                userId,           
                query.SearchTerm           
                );

            var resourceTypes = await this.resourceService.GetAllResourceTypes();

            AllResourcesQueryModel queryModel = new AllResourcesQueryModel();
            queryModel.Resources = myResourceServiceModel.Resources;
            queryModel.ResourceTypes = resourceTypes;         

            return View(queryModel);
        }


    }
}
