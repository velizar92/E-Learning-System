using Microsoft.AspNetCore.Mvc;

namespace E_LearningSystem.Web.Controllers
{
    public class TrainersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
