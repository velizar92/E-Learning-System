namespace E_LearningSystem.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Services.Services.Lectures.Models;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class LectureServiceTests
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
                .AddSingleton<ILectureService, LectureService>()
                .BuildServiceProvider();
        }

        [Test]
        public async Task AddLectureToCourse_Should_Add_Lecture_To_Course()
        {
            //Arrange          
            var lectureService = serviceProvider.GetService<ILectureService>();
            int courseId = 1;
            Lecture expLecture = new Lecture()
            {
                CourseId = 1,
                Name = "C# Lecture 1",
                Description = "Test lecture description.",
            };

            //Act
            await lectureService.AddLectureToCourse(courseId, "C# Lecture 1", "Test lecture description.", null);

            //Assert
            var lecture = sqliteDbContext.DbContext.Lectures.FirstOrDefault(l => l.CourseId == 1 && l.Name == "C# Lecture 1");
            Assert.AreEqual(expLecture.CourseId, lecture.CourseId);
            Assert.AreEqual(expLecture.Name, lecture.Name);
            Assert.AreEqual(expLecture.Description, lecture.Description);

        }

        [Test]
        public async Task AddLectureToCourse_Should_Return_Minus_One_If_Is_Passed_Invalid_CourseId()
        {
            //Arrange          
            var lectureService = serviceProvider.GetService<ILectureService>();
            int expectedResult = -1;
            int invalidCourseId = -1;

            //Act
            int actualResult = await lectureService.AddLectureToCourse(invalidCourseId, "C# Lecture 1", "Test lecture description.", null);

            //Assert

            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task DeleteLecture_Should_Delete_Lecture()
        {
            //Arrange
            var lectureService = serviceProvider.GetService<ILectureService>();
            int expectedCourseId = 1;
            bool expectedResult = true;
            int lectureId = 1;

            //Act
            var actualResult = await lectureService.DeleteLecture(lectureId);
            var lectureResult = sqliteDbContext.DbContext.Lectures.FirstOrDefault(l => l.Id == 1);

            Assert.AreEqual(null, lectureResult);
        }


        [Test]
        public async Task DeleteLecture_Should_Return_Correct_Positive_Result()
        {
            //Arrange
            var lectureService = serviceProvider.GetService<ILectureService>();
            int expectedCourseId = 1;
            bool expectedResult = true;
            int lectureId = 1;

            //Act
            var actualResult = await lectureService.DeleteLecture(lectureId);

            Assert.AreEqual(expectedResult, actualResult.Item1);
            Assert.AreEqual(expectedCourseId, actualResult.Item2);
        }


        [Test]
        public async Task DeleteLecture_Should_Return_Minus_One_If_Is_Passed_Invalid_LectureId()
        {
            //Arrange          
            var lectureService = serviceProvider.GetService<ILectureService>();
            bool expectedResult = false;
            int expectedCourseId = -1;
            int invalidLectureId = -1;

            //Act
            var actualResult = await lectureService.DeleteLecture(invalidLectureId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult.Item1);
            Assert.AreEqual(expectedCourseId, actualResult.Item2);
        }


        [Test]
        public async Task EditLecture_Should_Return_True_If_Lecture_Is_Edited()
        {
            //Arrange          
            var lectureService = serviceProvider.GetService<ILectureService>();
            bool expectedResult = true;

            //Act
            var actualResult = await lectureService.EditLecture(1, "Lecture 1", "Lecture 1 new description", new List<Resource> { });

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task EditLecture_Should_Edit_Lecture()
        {
            //Arrange          
            var lectureService = serviceProvider.GetService<ILectureService>();
            Lecture expectedLecture = new Lecture
            {
                Name = "Lecture 1",
                Description = "Lecture 1 new description",
                Resources = new List<Resource> {
                                new Resource
                                    {
                                        Id = 1,
                                        Name = "Video 1",
                                        ResourceTypeId = 2
                                    }
                }
            };

            //Act
            await lectureService.EditLecture(1, "Lecture 1", "Lecture 1 new description", new List<Resource> { });
            var actualLecture = sqliteDbContext.DbContext.Lectures.FirstOrDefault(l => l.Id == 1);

            //Assert
            Assert.AreEqual(expectedLecture.Name, actualLecture.Name);
            Assert.AreEqual(expectedLecture.Description, actualLecture.Description);
            Assert.AreEqual(expectedLecture.Resources.Count, actualLecture.Resources.Count);
        }

        [Test]
        public async Task EditLecture_Should_Return_False_If_Is_Passed_Invalid_LectureId()
        {
            //Arrange          
            var lectureService = serviceProvider.GetService<ILectureService>();
            bool expectedResult = false;
            int invalidLectureId = -1;

            //Act
            var actualResult = await lectureService.EditLecture(invalidLectureId, "Lecture 1", "Lecture 1 new description", new List<Resource> { });

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task GetLectureById_Should_Return_Correct_Lecture()
        {
            //Arrange
            var lectureService = serviceProvider.GetService<ILectureService>();
            int lectureId = 1;
            LectureServiceModel expLecture = new LectureServiceModel()
            {
                CourseId = 1,
                Name = "Lecture 1",
                Description = "Lecture 1 description",
            };

            //Act
            var actualLecture = await lectureService.GetLectureById(lectureId);

            //Assert
            Assert.AreEqual(expLecture.CourseId, actualLecture.CourseId);
            Assert.AreEqual(expLecture.Name, actualLecture.Name);
            Assert.AreEqual(expLecture.Description, actualLecture.Description);
        }


        [Test]
        public async Task GetLectureDetails_Should_Return_Correct_Lecture_Details()
        {
            //Arrange
            var lectureService = serviceProvider.GetService<ILectureService>();
            int lectureId = 1;
            LectureDetailsServiceModel expLectureDetails = new LectureDetailsServiceModel()
            {
                CourseId = 1,
                Name = "Lecture 1",
                Description = "Lecture 1 description",
                ResourceUrls = new[] { "Video 1" }
            };

            //Act
            var actualLectureDetails = await lectureService.GetLectureDetails(lectureId);

            //Assert
            Assert.AreEqual(expLectureDetails.CourseId, actualLectureDetails.CourseId);
            Assert.AreEqual(expLectureDetails.Name, actualLectureDetails.Name);
            Assert.AreEqual(expLectureDetails.Description, actualLectureDetails.Description);
            for (int i = 0; i < expLectureDetails.ResourceUrls.Length; i++)
            {
                Assert.AreEqual(expLectureDetails.ResourceUrls[i], actualLectureDetails.ResourceUrls[i]);
            }
        }


        [Test]
        public async Task GetLectureIdByResourceId_Should_Return_Correct_Lecture()
        {
            //Arrange
            var lectureService = serviceProvider.GetService<ILectureService>();
            int expectedLectureId = 1;
            int resourceId = 1;

            //Act
            int actualLectureId = await lectureService.GetLectureIdByResourceId(resourceId);

            //Assert
            Assert.AreEqual(expectedLectureId, actualLectureId);
        }


        [Test]
        public async Task GetLectureIdByResourceId_Should_Return_MinusOne_If_Is_Passed_Invalid_ResourceId()
        {
            //Arrange
            var lectureService = serviceProvider.GetService<ILectureService>();
            int expectedLectureId = -1;
            int invalidResourceId = -1;

            //Act
            int actualResultId = await lectureService.GetLectureIdByResourceId(invalidResourceId);

            //Assert
            Assert.AreEqual(expectedLectureId, actualResultId);
        }
    }
}

