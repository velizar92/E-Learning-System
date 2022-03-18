namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Infrastructure.Helpers;
    using E_LearningSystem.Services.Services.ShoppingCarts.Models;
    using AspNetCoreHero.ToastNotification.Abstractions;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;
    using static E_LearningSystem.Infrastructure.Constants.MessageConstants;

    public class CartsController : Controller
    {

        private readonly IShoppingCartService shoppingCartService;
        private readonly ICourseService courseService;
        private readonly INotyfService notyfService;
        private readonly UserManager<User> userManagerService;

        private const string sessionKey = "cart";


        public CartsController(
            IShoppingCartService shoppingCartService,
            ICourseService courseService,
            INotyfService notyfService,
            UserManager<User> userManagerService)
        {
            this.shoppingCartService = shoppingCartService;
            this.courseService = courseService;
            this.notyfService = notyfService;
            this.userManagerService = userManagerService;
        }


        [Authorize]
        public IActionResult Details()
        {
            var cartItems = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, sessionKey);

            if (TempData[SuccessMessage] != null)
            {
                this.notyfService.Success($"{TempData[SuccessMessage]}");
            }
           
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

            if (SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, sessionKey) == null)
            {
                List<ItemServiceModel> cart = new List<ItemServiceModel>();
                cart.Add(new ItemServiceModel { Course = course, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, sessionKey, cart);
            }
            else
            {
                List<ItemServiceModel> cart = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, sessionKey);
                int index = isExist(courseId);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new ItemServiceModel { Course = course, Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, sessionKey, cart);
            }

            TempData[SuccessMessage] = "Course has been added to cart.";

            return RedirectToAction(nameof(Details));
        }



        [Authorize(Roles = LearnerRole)]
        public IActionResult RemoveCourseFromCart(int courseId)
        {
            List<ItemServiceModel> cart = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, sessionKey);
            int index = isExist(courseId);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, sessionKey, cart);

            return RedirectToAction(nameof(Details));
        }


        [Authorize(Roles = LearnerRole)]
        public async Task<IActionResult> BuyCourses()
        {
            var user = await userManagerService.GetUserAsync(HttpContext.User);

            List<ItemServiceModel> cartItems = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, sessionKey);
            var errors = await this.shoppingCartService.BuyCourses(cartItems, user);

            if (errors.Count() > 0)
            {
                var errorsModel = string.Join('\n', errors);
                TempData[ErrorMessage] = errorsModel;

                return RedirectToAction(nameof(Details));
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, sessionKey, null);

            TempData[SuccessMessage] = "You successfuly buy all courses.";
            return RedirectToAction("Index", "Home");
        }


        private int isExist(int id)
        {
            List<ItemServiceModel> cart = SessionHelper.GetObjectFromJson<List<ItemServiceModel>>(HttpContext.Session, sessionKey);
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
