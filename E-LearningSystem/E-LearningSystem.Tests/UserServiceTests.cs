

namespace E_LearningSystem.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;
    using E_LearningSystem.Services.Services.Users;
    using Microsoft.Extensions.DependencyInjection;
   
    public class UserServiceTests
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
                .AddSingleton<IUserService, UserService>()
                .BuildServiceProvider();
        }

        [Test]
        public async Task CheckIfUserHasCourse_Should_Return_True_If_User_Has_Course()
        {
            //Arrange          
            bool expectedResult = true;
            var userService = serviceProvider.GetService<IUserService>();
            string userId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5";
            int courseId = 1;

            //Act
            var actualResult = await userService.CheckIfUserHasCourse(userId, courseId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task CheckIfUserHasCourse_Should_Return_False_If_User_Doesnt_Have_Course()
        {
            //Arrange          
            bool expectedResult = false;
            var userService = serviceProvider.GetService<IUserService>();
            string userId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5";
            int courseId = 2;

            //Act
            var actualResult = await userService.CheckIfUserHasCourse(userId, courseId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


    }
}
