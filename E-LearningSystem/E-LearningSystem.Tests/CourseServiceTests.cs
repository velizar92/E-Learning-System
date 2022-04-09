namespace E_LearningSystem.Tests
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Services.Services.Courses.Models;
    using E_LearningSystem.Services.Services.Lectures.Models;

    public class CourseServiceTests
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
                .AddSingleton<ICourseService, CourseService>()
                .BuildServiceProvider();
        }


        [Test]
        public void CheckIfCourseCategoryExists_Should_Return_True_If_Category_With_Such_Id_Exists()
        {
            //Arrange          
            bool expectedResult = true;
            var courseService = serviceProvider.GetService<ICourseService>();
            int categoryId = 1;


            //Act
            var actualResult = courseService.CheckIfCourseCategoryExists(categoryId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void CheckIfCourseCategoryExists_Should_Return_False_If_Category_With_Such_Id_Doesnt_Exists()
        {
            //Arrange          
            bool expectedResult = false;
            var courseService = serviceProvider.GetService<ICourseService>();
            int invalidCategoryId = 4;


            //Act
            var actualResult = courseService.CheckIfCourseCategoryExists(invalidCategoryId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task CreateCourse_Should_Create_Course()
        {
            //Arrange                      
            var courseService = serviceProvider.GetService<ICourseService>();
            string expectedCourseName = "C# Test Course";
            int expectedTrainerId = 1;

            //Act
            var actualResult = await courseService.CreateCourse(
                    "EC90AD4D-7C94-4BAC-B04C-331431D132D5",
                    1,
                    "C# Test Course",
                    "Sample description for this interesting course",
                    122.00,
                    1,
                    "testPictureFileName"
                );

            var actualCourse = await sqliteDbContext.DbContext.Courses.FirstOrDefaultAsync(c => c.Name == "C# Test Course" && c.TrainerId == 1);

            //Assert
            Assert.AreEqual(expectedCourseName, actualCourse.Name);
            Assert.AreEqual(expectedTrainerId, actualCourse.TrainerId);
        }


        [Test]
        public async Task CreateCourse_Should_Return_Correct_CourseId()
        {
            //Arrange                      
            var courseService = serviceProvider.GetService<ICourseService>();
            string expectedCourseName = "C# Test Course";
            int expectedTrainerId = 1;

            //Act
            var returnedCourseId = await courseService.CreateCourse(
                    "EC90AD4D-7C94-4BAC-B04C-331431D132D5",
                    1,
                    "C# Test Course",
                    "Sample description for this interesting course",
                    122.00,
                    1,
                    "testPictureFileName"
                );

            var actualCourse = await sqliteDbContext.DbContext.Courses.FirstOrDefaultAsync(c => c.Name == "C# Test Course" && c.TrainerId == 1);

            //Assert
            Assert.AreEqual(actualCourse.Id, returnedCourseId);
        }



        [Test]
        public async Task DeleteCourse_Should_Delete_Course()
        {
            //Arrange
            int courseId = 1;
            bool expectedExisting = false;
            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualResult = await courseService.DeleteCourse(courseId);

            bool isExists = sqliteDbContext.DbContext.Courses.Any(c => c.Name == "C# Basics" && c.TrainerId == 1);

            //Assert
            Assert.AreEqual(expectedExisting, isExists);
        }


        [Test]
        public async Task DeleteCourse_Should_Return_True_If_Course_Is_Deleted()
        {
            //Arrange
            int courseId = 1;
            bool expectedResult = true;
            string expectedErrorMessage = null;

            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualResult = await courseService.DeleteCourse(courseId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult.Item1);
            Assert.AreEqual(expectedErrorMessage, actualResult.Item2);
        }


        [Test]
        public async Task DeleteCourse_Should_Return_False_And_Proper_Error_Message_If_Is_Passed_Invalid_CourseId()
        {
            //Arrange
            int invalidCourseId = -1;
            bool expectedBoolResult = false;
            string expectedErrorMessage = "Unknown course.";

            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualResult = await courseService.DeleteCourse(invalidCourseId);

            //Assert
            Assert.AreEqual(expectedBoolResult, actualResult.Item1);
            Assert.AreEqual(expectedErrorMessage, actualResult.Item2);
        }


        [Test]
        public async Task EditCourse_Should_Edit_Course()
        {
            //Arrange
            string expectedCourseName = "C# Basics Updated";
            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualResult = await courseService.EditCourse(1, "C# Basics Updated", "CourseDescripton", 100.00, 1, "PictureName");

            var course = await sqliteDbContext.DbContext.Courses.FirstOrDefaultAsync(c => c.Id == 1);

            //Assert
            Assert.AreEqual(expectedCourseName, course.Name);
        }


        [Test]
        public async Task EditCourse_Should_Return_True_Without_ErrorMessage_If_Course_Is_Deleted()
        {
            //Arrange
            bool expectedResult = true;
            string expectedErrorMessage = null;
            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualResult = await courseService.EditCourse(1, "C# Basics Updated", "CourseDescripton", 100.00, 1, "PictureName");

            //Assert
            Assert.AreEqual(expectedResult, actualResult.Item1);
            Assert.AreEqual(expectedErrorMessage, actualResult.Item2);
        }


        [Test]
        public async Task EditCourse_Should_Return_False_And_Proper_Error_Message_If_Is_Passed_Invalid_CourseId()
        {
            //Arrange
            int invalidCourseId = -1;
            bool expectedBoolResult = false;
            string expectedErrorMessage = "Unknown course.";

            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualResult = await courseService.EditCourse(invalidCourseId, "CourseName", "CourseDescripton", 100.00, 1, "PictureName");

            //Assert
            Assert.AreEqual(expectedBoolResult, actualResult.Item1);
            Assert.AreEqual(expectedErrorMessage, actualResult.Item2);
        }


        [Test]
        public async Task GetAllCourseCategories_Should_Return_All_CourseCategories()
        {
            //Arrange
            List<CourseCategoriesServiceModel> expectedCourseCategories = new List<CourseCategoriesServiceModel>()
            {
                new CourseCategoriesServiceModel{ Id = 1,   Name = "Programming"},
                new CourseCategoriesServiceModel{ Id = 2,   Name = "Networking"},
                new CourseCategoriesServiceModel{ Id = 3,   Name = "Algorithms"},
            };

            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualCourseCategories = await courseService.GetAllCourseCategories();
            var actualCourseCategoriesList = actualCourseCategories.ToList();

            //Assert
            for (int i = 0; i < expectedCourseCategories.Count(); i++)
            {
                Assert.AreEqual(expectedCourseCategories[i].Id, actualCourseCategoriesList[i].Id);
                Assert.AreEqual(expectedCourseCategories[i].Name, actualCourseCategoriesList[i].Name);
            }
        }


        [Test]
        public async Task GetAllCourses_Should_Return_All_Courses()
        {
            //Arrange
            int expectedCoursesCount = 1;
            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualCourses = await courseService.GetAllCourses();

            //Assert
            Assert.AreEqual(expectedCoursesCount, actualCourses.Count());
        }


        [Test]
        public async Task GetCourseById_Should_Return_Correct_Course()
        {
            //Arrange
            int courseId = 1;
            CourseServiceModel expectedCourse = new CourseServiceModel
            {
                Id = 1,
                Name = "C# Basics",
                Description = "C# basics description",
                Price = 100.00,
                ImageUrl = "testUrl",
            };

            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualCourse = await courseService.GetCourseById(courseId);

            //Assert
            Assert.AreEqual(expectedCourse.Id, actualCourse.Id);
            Assert.AreEqual(expectedCourse.Name, actualCourse.Name);
            Assert.AreEqual(expectedCourse.Description, actualCourse.Description);
            Assert.AreEqual(expectedCourse.Price, actualCourse.Price);
            Assert.AreEqual(expectedCourse.ImageUrl, actualCourse.ImageUrl);
        }


        [Test]
        public async Task GetCourseCreatorId_Should_Return_Correct_TrainerId()
        {
            //Arrange
            int expectedTrainerId = 1;
            int courseId = 1;
            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            int actualTrainerId = await courseService.GetCourseCreatorId(courseId);

            //Assert
            Assert.AreEqual(expectedTrainerId, actualTrainerId);
        }


        [Test]
        public async Task GetMyCourses_With_Passed_UserId_Should_Return_Count_Of_My_Courses()
        {
            //Arrange
            int expectedCoursesCount = 1;
            string userId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5";
            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var myCourses = await courseService.GetMyCourses(userId);
            int actualCoursesCount = myCourses.Count();

            //Assert
            Assert.AreEqual(expectedCoursesCount, actualCoursesCount);
        }


        [Test]
        public async Task GetMyCourses_With_Passed_TrainerId_Should_Return_Count_Of_My_Courses()
        {
            //Arrange
            int expectedCoursesCount = 1;
            int trainerId = 1;
            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var myCourses = await courseService.GetMyCourses(trainerId);
            int actualCoursesCount = myCourses.Count();

            //Assert
            Assert.AreEqual(expectedCoursesCount, actualCoursesCount);
        }



        [Test]
        public async Task GetCourseDetails_Should_Get_Course_Details()
        {
            //Arrange
            CourseDetailsServiceModel expectedCourseDetails = new CourseDetailsServiceModel
            {
                Id = 1,
                Name = "C# Basics",
                Description = "C# basics description",
                Price = 100.00,
                ImageUrl = "testUrl",
                Lectures = new List<LectureServiceModel>
                        {
                            new LectureServiceModel
                            {
                                Id = 1,
                                Name = "Lecture 1",
                                Description = "Lecture 1 description",
                            },
                            new LectureServiceModel
                            {
                                Id = 2,
                                Name = "Lecture 2",
                                Description = "Lecture 2 description",
                            },
                            new LectureServiceModel
                            {
                                Id = 3,
                                Name = "Lecture 3",
                                Description = "Lecture 3 description",
                            }
                        }
            };

            int courseId = 1;
            var courseService = serviceProvider.GetService<ICourseService>();

            //Act
            var actualCourseDetails = await courseService.GetCourseDetails(courseId);

            //Assert
            Assert.AreEqual(expectedCourseDetails.Id, actualCourseDetails.Id);
            Assert.AreEqual(expectedCourseDetails.Name, actualCourseDetails.Name);
            Assert.AreEqual(expectedCourseDetails.Description, actualCourseDetails.Description);
            Assert.AreEqual(expectedCourseDetails.Price, actualCourseDetails.Price);
            Assert.AreEqual(expectedCourseDetails.ImageUrl, actualCourseDetails.ImageUrl);

            for (int i = 0; i < expectedCourseDetails.Lectures.Count(); i++)
            {
                Assert.AreEqual(expectedCourseDetails.Lectures.ElementAt(i).Id, actualCourseDetails.Lectures.ElementAt(i).Id);
                Assert.AreEqual(expectedCourseDetails.Lectures.ElementAt(i).Name, actualCourseDetails.Lectures.ElementAt(i).Name);
                Assert.AreEqual(expectedCourseDetails.Lectures.ElementAt(i).Description, actualCourseDetails.Lectures.ElementAt(i).Description);
            }   
        }
    }
}
