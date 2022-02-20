using Microsoft.AspNetCore.Mvc;

namespace E_LearningSystem.Web.Controllers
{
    public class ResourcesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
