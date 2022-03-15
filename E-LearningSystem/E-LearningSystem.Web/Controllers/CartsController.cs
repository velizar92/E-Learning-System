namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;

    using static E_LearningSystem.Infrastructure.IdentityConstants;

    public class CartsController : Controller
    {

        private readonly IShoppingCartService shoppingCartService;
        private readonly UserManager<User> userManagerService;


        public CartsController(IShoppingCartService shoppingCartService, UserManager<User> userManagerService)
        {
            this.shoppingCartService = shoppingCartService;
            this.userManagerService = userManagerService;
        }


        public IActionResult Details(string id)
        {
            var cartDetails = this.shoppingCartService.GetCartDetails(id);

            return View(cartDetails);
        }

      
        [HttpPost]
        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> AddCourseToCart(string shoppingCartId, int courseId)
        {
            bool isAdded = await this.shoppingCartService.AddCourseToCart(shoppingCartId, courseId);

            if (isAdded == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Details), new { shoppingCartId });
        }


        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> DeleteCourseFromCart(string shoppingCartId, int courseId)
        {
            bool isDeleted = await this.shoppingCartService.DeleteCourseFromCart(shoppingCartId, courseId);

            if(isDeleted == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Details), new { shoppingCartId });
        }


        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> BuyCourses(string shoppingCartId)
        {
            var user = await userManagerService.GetUserAsync(HttpContext.User);
            bool areBuyed = await this.shoppingCartService.BuyCourses(shoppingCartId, user);

            if (areBuyed == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Details), new { shoppingCartId });
        }
    }
}
