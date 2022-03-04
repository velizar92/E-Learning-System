namespace E_LearningSystem.Web.Controllers
{
    using E_LearningSystem.Services.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static E_LearningSystem.Infrastructure.IdentityConstants;

    public class CartsController : Controller
    {

        private readonly IShoppingCartService shoppingCartService;


        public CartsController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }


        public IActionResult Details(string id)
        {
            var cartDetails = this.shoppingCartService.GetCartDetails(id);

            return View(cartDetails);
        }


        [HttpPost]
        [Authorize(Roles = AdminRole)]
        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> AddCourseToCart(string shoppingCartId, int courseId)
        {
            bool isAdded = await this.shoppingCartService.AddCourseToCart(shoppingCartId, courseId);

            //TO DO Check...

            return RedirectToAction(nameof(Details), new { shoppingCartId });
        }

        [Authorize(Roles = AdminRole)]
        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> DeleteCourseFromCart(string shoppingCartId, int courseId)
        {
            bool isDeleted = await this.shoppingCartService.DeleteCourseFromCart(shoppingCartId, courseId);

            //TO DO Check...

            return RedirectToAction(nameof(Details), new { shoppingCartId });
        }


        [Authorize(Roles = AdminRole)]
        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> BuyCourses(string shoppingCartId)
        {
            bool areBuyed = await this.shoppingCartService.BuyCourses(shoppingCartId);

            //TO DO Check...

            return RedirectToAction(nameof(Details), new { shoppingCartId });
        }
    }
}
