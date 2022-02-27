namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ELearningSystemDbContext dbContext;

        public ShoppingCartService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }


        public async Task<bool> AddCourseToCart(int _shoppingCartId, int _courseId)
        {
            var shoppingCart = await GetCartById(_shoppingCartId);

            var course = await dbContext
                               .Courses
                               .FirstOrDefaultAsync(c => c.Id == _courseId);

            if (shoppingCart == null || course == null)
            {
                return false;
            }
      
            shoppingCart.Courses.Add(course);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> BuyCourses(int _shoppingCartId)
        {
            var shoppingCart = await GetCartById(_shoppingCartId);

            if (shoppingCart == null)
            {
                return false;
            }

            shoppingCart.Courses.Clear();
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteCourseFromCart(int _shoppingCartId, int _courseId)
        {
             var shoppingCart = await GetCartById(_shoppingCartId);

             var course = await dbContext
                               .Courses
                               .FirstOrDefaultAsync(c => c.Id == _courseId);

            if (shoppingCart == null || course == null)
            {
                return false;
            }

            shoppingCart.Courses.Remove(course);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<ShoppingCart> GetCartById(int _shoppingCartId)
        {
            var shoppingCart = await dbContext
                                .ShoppingCarts
                                .FirstOrDefaultAsync(sc => sc.Id == _shoppingCartId);

            return shoppingCart;
        }


        public async Task<ShoppingCartDetailsServiceModel> GetCartDetails(int _shoppingCartId)
        {
            var shoppingCart = await GetCartById(_shoppingCartId);

            if(shoppingCart != null)
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


    }
}
