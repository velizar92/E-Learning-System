namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Data.Models;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.ShoppingCarts.Models;

    using static E_LearningSystem.Infrastructure.Messages.ErrorMessages;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ELearningSystemDbContext dbContext;


        public ShoppingCartService(ELearningSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<IEnumerable<string>> BuyCourses(List<ItemServiceModel> cartItems, User user)
        {
            List<string> errors = new List<string>();

            if (cartItems != null && cartItems.Count > 0)
            {
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
                    }
                    else
                    {
                        errors.Add(string.Format(CourseAlreadyAdded, cartItem.Course.Name));
                    }
                }
            }
            else
            {
                errors.Add(EmptyShoppingCart);
            }
         
            await dbContext.SaveChangesAsync();
            return errors;
        }


    }
}
