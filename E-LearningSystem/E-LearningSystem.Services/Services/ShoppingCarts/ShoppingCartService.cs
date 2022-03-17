namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Data.Models;
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
            bool isBuyed = false;

            foreach (var cartItem in cartItems)
            {
                if (dbContext.CourseUsers.FirstOrDefault(c => (c.CourseId == cartItem.Course.Id) && (c.UserId == user.Id)) == null)
                {
                    var course = new Course
                    {
                        Id = cartItem.Course.Id,
                        Name = cartItem.Course.Name,
                        Description = cartItem.Course.Description,
                        Price = cartItem.Course.Price,
                        CourseCategoryId = cartItem.Course.CategoryId,
                        ImageUrl = cartItem.Course.ImageUrl,
                        AssignedStudents = cartItem.Course.AssignedStudents + 1
                    };

                    dbContext.CourseUsers.Add(new CourseUser { CourseId = course.Id, UserId = user.Id });
                    isBuyed = true;
                }
            }

            await dbContext.SaveChangesAsync();
            return isBuyed;
        }


    }
}
