namespace E_LearningSystem.Web.Areas.Admin.Controllers
{ 
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;

    [Authorize(Roles = AdminRole)]
    [Area("Admin")]
    public abstract class AdminController : Controller
    {
       
    }
}
