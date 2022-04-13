namespace E_LearningSystem.Tests
{ 
    using NUnit.Framework;
    using Microsoft.Extensions.DependencyInjection;
    using E_LearningSystem.Services.Services;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    public class ResourceServiceTests
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
                .AddSingleton<IResourceService, ResourceService>()
                .BuildServiceProvider();
        }

        [Test]
        public void CheckIfResourceTypeExists_Should_Return_False_In_Case_ResourceType_Does_Not_Exists()
        {
            //Arrange
            var resourceService = serviceProvider.GetService<IResourceService>();
            int invalidResourceTypeId = -1;
            bool expectedExisting = false;
           
            //Act
            var actualExisting =  resourceService.CheckIfResourceTypeExists(invalidResourceTypeId);
           
            //Assert
            Assert.AreEqual(expectedExisting, actualExisting);
        }


        [Test]
        public void CheckIfResourceTypeExists_Should_Return_True_In_Case_ResourceType_Exists()
        {
            //Arrange
            var resourceService = serviceProvider.GetService<IResourceService>();
            int resourceTypeId = 1;
            bool expectedExisting = true;

            //Act
            var actualExisting = resourceService.CheckIfResourceTypeExists(resourceTypeId);

            //Assert
            Assert.AreEqual(expectedExisting, actualExisting);
        }


        [Test]
        public async Task GetAllResourceTypes_Should_Return_All_ResourceTypes()
        {
            //Arrange
            var resourceService = serviceProvider.GetService<IResourceService>();
            IEnumerable<string> expectedResourceTypes = new List<string>()
            {
               "PPT Presentation",
               "Video MP4",
               "PDF Document"
            };

            //Act
            var actualResourceTypes = await resourceService.GetAllResourceTypes();

            //Assert
            for (int i = 0; i < expectedResourceTypes.Count(); i++)
            {
                Assert.AreEqual(expectedResourceTypes.ElementAt(i), actualResourceTypes.ElementAt(i));
            }
        }

    }
}
