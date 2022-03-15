namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Courses.Models;


    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ELearningSystemDbContext dbContext;


        public ShoppingCartService(ELearningSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<bool> AddCourseToCart(string shoppingCartId, int courseId)
        {
            var shoppingCart = await GetCartById(shoppingCartId);

            var course = await dbContext
                               .Courses
                               .FirstOrDefaultAsync(c => c.Id == courseId);

            if (shoppingCart == null || course == null)
            {
                return false;
            }

            shoppingCart.Courses.Add(course);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> BuyCourses(string shoppingCartId, User user)
        {
            var shoppingCart = await GetCartById(shoppingCartId);

            if (shoppingCart == null || user == null)
            {
                return false;
            }

            foreach (var course in shoppingCart.Courses)
            {
                user.Courses.Add(course);
            }

            shoppingCart.Courses.Clear();
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteCourseFromCart(string shoppingCartId, int courseId)
        {
            var shoppingCart = await GetCartById(shoppingCartId);

            var course = await dbContext
                              .Courses
                              .FirstOrDefaultAsync(c => c.Id == courseId);

            if (shoppingCart == null || course == null)
            {
                return false;
            }

            shoppingCart.Courses.Remove(course);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<ShoppingCart> GetCartById(string shoppingCartId)
        {
            var shoppingCart = await dbContext
                                .ShoppingCarts
                                .FirstOrDefaultAsync(sc => sc.Id == shoppingCartId);

            return shoppingCart;
        }


        public async Task<ShoppingCartDetailsServiceModel> GetCartDetails(string shoppingCartId)
        {
            var shoppingCart = await GetCartById(shoppingCartId);

            if (shoppingCart != null)
            {
                return new ShoppingCartDetailsServiceModel()
                {
                    CartId = shoppingCart.Id,
                    Courses = shoppingCart.Courses.Select(c => new CourseDetailsServiceModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        ImageUrl = c.ImageUrl
                    }),
                };
            }

            return null;
        }


        public async Task<string> GetCartIdByUserId(string userId)
        {
            var cart = await dbContext.ShoppingCarts.Where(sc => sc.UserId == userId).FirstOrDefaultAsync();

            return cart.Id;
        }



    }
}
