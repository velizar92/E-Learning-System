namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
