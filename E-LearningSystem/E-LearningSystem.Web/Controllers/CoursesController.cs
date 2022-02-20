using Microsoft.AspNetCore.Mvc;

namespace E_LearningSystem.Web.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
