namespace E_LearningSystem.Tests
{
    using NUnit.Framework;
    using Microsoft.Extensions.DependencyInjection;
    using E_LearningSystem.Services.Services;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Resources.Models;

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
            var actualExisting = resourceService.CheckIfResourceTypeExists(invalidResourceTypeId);

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


        [Test]
        public async Task DeleteResource_Should_Return_True_If_Resource_Is_Deleted()
        {
            //Arrange
            var resourceService = serviceProvider.GetService<IResourceService>();
            int resourceId = 1;
            bool expectedResult = true;

            //Act
            bool actualResult = await resourceService.DeleteResource(resourceId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task DeleteResource_Should_Delete_Resource()
        {
            //Arrange
            var resourceService = serviceProvider.GetService<IResourceService>();
            int resourceId = 1;
            Resource expectedResource = null;

            //Act
            await resourceService.DeleteResource(resourceId);

            var searchedResource = sqliteDbContext.DbContext.Resources.FirstOrDefault(r => r.Id == 1);

            //Assert
            Assert.AreEqual(expectedResource, searchedResource);
        }


        [Test]
        public async Task DeleteResource_Should_Return_False_If_Passed_ReosurceId_Is_Invalid()
        {
            //Arrange
            var resourceService = serviceProvider.GetService<IResourceService>();
            int invalidResourceId = -1;
            bool expectedResult = false;

            //Act
            bool actualResult = await resourceService.DeleteResource(invalidResourceId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task GetAllMyResources_Should_Return_All_My_Resources()
        {
            //Arrange
            var resourceService = serviceProvider.GetService<IResourceService>();
            string userId = "EEEEEEEE-6666-6666-6666-331431D13211";

            ResourceQueryServiceModel expectedResources = new ResourceQueryServiceModel
            {
                Resources = new List<AllResourcesServiceModel>()
                {
                    new AllResourcesServiceModel
                    {
                         ResourceName = "Video 1",
                         LectureName = "Lecture 1"
                    }
                }
            };

            //Act
            var actualResult = await resourceService.GetAllMyResources(userId, null);

            //Assert
            for (int i = 0; i < expectedResources.Resources.Count(); i++)
            {
                Assert.AreEqual(expectedResources.Resources.ElementAt(i).ResourceName, actualResult.Resources.ElementAt(i).ResourceName);
                Assert.AreEqual(expectedResources.Resources.ElementAt(i).LectureName, actualResult.Resources.ElementAt(i).LectureName);
            }
        }
    }
}
