namespace E_LearningSystem.Web.Controllers
{
    using E_LearningSystem.Services.Services;
    using Microsoft.AspNetCore.Mvc;

    public class CartsController : Controller
    {

        private readonly IShoppingCartService shoppingCartService;


        public CartsController(IShoppingCartService _shoppingCartService)
        {
            this.shoppingCartService = _shoppingCartService;
        }


        public IActionResult Details(int _id)
        {
            var cartDetails = this.shoppingCartService.GetCartDetails(_id);

            return View(cartDetails);
        }


        [HttpPost]
        public async Task<IActionResult> AddCourseToCart(int _shoppingCartId, int _courseId)
        {
            bool isAdded = await this.shoppingCartService.AddCourseToCart(_shoppingCartId, _courseId);

            //TO DO Check...

            return RedirectToAction(nameof(Details), new { _shoppingCartId });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCourseFromCart(int _shoppingCartId, int _courseId)
        {
            bool isDeleted = await this.shoppingCartService.DeleteCourseFromCart(_shoppingCartId, _courseId);

            //TO DO Check...

            return RedirectToAction(nameof(Details), new { _shoppingCartId });
        }


        [HttpPost]
        public async Task<IActionResult> BuyCourses(int _shoppingCartId)
        {
            bool areBuyed = await this.shoppingCartService.BuyCourses(_shoppingCartId);

            //TO DO Check...

            return RedirectToAction(nameof(Details), new { _shoppingCartId });
        }
    }
}
