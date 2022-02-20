using Microsoft.AspNetCore.Mvc;

namespace E_LearningSystem.Web.Controllers
{
    public class LecturesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
