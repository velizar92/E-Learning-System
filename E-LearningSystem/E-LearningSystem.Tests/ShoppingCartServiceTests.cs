namespace E_LearningSystem.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Services.Services.ShoppingCarts.Models;
    using E_LearningSystem.Services.Services.Courses.Models;
    
    using static E_LearningSystem.Infrastructure.Messages.ErrorMessages;

    public class ShoppingCartServiceTests
    {
        private ServiceProvider serviceProvider;
        private SqliteDbContext sqliteDbContext;

        [SetUp]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            sqliteDbContext = new SqliteDbContext();

            serviceProvider = serviceCollection
                .AddSingleton(sp => sqliteDbContext.DbContext)
                .AddSingleton<IShoppingCartService, ShoppingCartService>()
                .BuildServiceProvider();
        }

        [Test]
        public async Task BuyCourses_Should_Add_Courses_To_User()
        {
            //Arrange          
            var cartService = serviceProvider.GetService<IShoppingCartService>();
            string userId = "EEEEEEEE-6666-6666-6666-331431D13211";

            List<ItemServiceModel> cartItems = new List<ItemServiceModel>()
            {
                new ItemServiceModel
                {
                    Course = new CourseServiceModel
                    {
                        Id = 1,
                        Name = "C# Basics",
                        Description = "C# basics description",
                        Price = 100.00,
                        ImageUrl = "testUrl",
                    }
                }               
            };

            //Act
            var user = await sqliteDbContext.DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            await cartService.BuyCourses(cartItems, user);

            //Assert
            if (sqliteDbContext.DbContext.CourseUsers.Any(c => c.CourseId == 7 && c.UserId == userId) == true)
            {
                Assert.Pass();
            }
        }


        [Test]
        public async Task BuyCourses_Should_Return_Correct_Error_Message_In_Case_Shopping_Cart_Is_Empty()
        {
            //Arrange          
            var cartService = serviceProvider.GetService<IShoppingCartService>();
            string userId = "EEEEEEEE-6666-6666-6666-331431D13211";
            IEnumerable<string> expectedErrors = new List<string>()
            {
                EmptyShoppingCart
            };

            List<ItemServiceModel> cartItems = new List<ItemServiceModel>(){};

            //Act
            var user = await sqliteDbContext.DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var actualErrors = await cartService.BuyCourses(cartItems, user);

            //Assert
            for (int i = 0; i < expectedErrors.Count(); i++)
            {
                Assert.AreEqual(expectedErrors.ElementAt(i), actualErrors.ElementAt(i));
            }        
        }
    }
}
