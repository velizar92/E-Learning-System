namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Infrastructure.Helpers;
    using E_LearningSystem.Services.Services.ShoppingCarts.Models;

    using static E_LearningSystem.Infrastructure.IdentityConstants;
 
    public class CartsController : Controller
    {

        private readonly IShoppingCartService shoppingCartService;
        private readonly ICourseService courseService;
        private readonly UserManager<User> userManagerService;


        public CartsController(
            IShoppingCartService shoppingCartService,
            ICourseService courseService,
            UserManager<User> userManagerService)
        {
            this.shoppingCartService = shoppingCartService;
            this.courseService = courseService;
            this.userManagerService = userManagerService;
        }


        [Authorize]
        public IActionResult Details()
        {        
            var cartItems = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, "cart");
            if (cartItems != null)
            {
                ViewBag.totalItemsSum = cartItems.Sum(item => item.Course.Price * item.Quantity);
            }
              
            return View(cartItems);
        }


        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> AddCourseToCart(int courseId)
        {          
            var course = await this.courseService.GetCourseById(courseId);

            if (SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, "cart") == null)
            {
                List<ItemServiceModel> cart = new List<ItemServiceModel>();
                cart.Add(new ItemServiceModel { Course = course, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<ItemServiceModel> cart = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, "cart");
                int index = isExist(courseId);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new ItemServiceModel { Course = course, Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction(nameof(Details));
        }



        [Authorize(Roles = LearnerRole)]
        public IActionResult RemoveCourseFromCart(int courseId)
        {
            List<ItemServiceModel> cart = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, "cart");
            int index = isExist(courseId);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return RedirectToAction(nameof(Details));
        }



        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> BuyCourses()
        {
            var user = await userManagerService.GetUserAsync(HttpContext.User);        

            List<ItemServiceModel> cartItems = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, "cart");
            bool areBuyed  = await this.shoppingCartService.BuyCourses(cartItems, user);

            if (areBuyed == false)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Home");

        }


        private int isExist(int id)
        {
            List<ItemServiceModel> cart = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, "cart");
            for (int courseIndex = 0; courseIndex < cart.Count; courseIndex++)
            {
                if (cart[courseIndex].Course.Id.Equals(id))
                {
                    return courseIndex;
                }
            }
            return -1;
        }
    }
}
