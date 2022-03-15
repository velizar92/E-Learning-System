namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.ShoppingCarts.Models;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ELearningSystemDbContext dbContext;


        public ShoppingCartService(ELearningSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        public async Task<bool> BuyCourses(List<ItemServiceModel> cartItems, User user)
        {
            
            foreach (var courseItem in cartItems)
            {
                var course = new Course
                {
                    Name = courseItem.Course.Name,
                    Description = courseItem.Course.Description,
                    Price = courseItem.Course.Price,
                    CourseCategoryId = courseItem.Course.CategoryId,
                    ImageUrl = courseItem.Course.ImageUrl
                };

                user.Courses.Add(course);
            }
         
            await dbContext.SaveChangesAsync();
            return true;
        }


    }
}
