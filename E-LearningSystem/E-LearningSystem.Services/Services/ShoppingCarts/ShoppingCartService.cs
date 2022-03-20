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
                        var course = dbContext.Courses.FirstOrDefault(c => c.Id == cartItem.Course.Id);
                        dbContext.CourseUsers.Add(new CourseUser { CourseId = course.Id, UserId = user.Id });
                        
                        if(course.AssignedStudents == null)
                        {
                            course.AssignedStudents = 0;
                        }

                        course.AssignedStudents = course.AssignedStudents + 1;       
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
