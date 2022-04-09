namespace E_LearningSystem.Tests
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using E_LearningSystem.Services.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Services.Services.Issues.Models;

    public class IssueServiceTests
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
                .AddSingleton<IIssueService, IssueService>()
                .BuildServiceProvider();
        }


        [Test]
        public async Task CreateIssue_Should_Create_Issue()
        {
            //Arrange                      
            var issueService = serviceProvider.GetService<IIssueService>();
            string expectedIssueTitle = "Test issue title";
            string expectedIssueDescription = "Test issue description";

            //Act
            var actualResult = await issueService.CreateIssue(
                    "EEEEEEEE-6666-6666-6666-331431D13211",
                    1,
                    expectedIssueTitle,
                    expectedIssueDescription
                );

            var actualIssue = await sqliteDbContext.DbContext.Issues.FirstOrDefaultAsync(c => c.Title == expectedIssueTitle && c.Description == expectedIssueDescription);

            //Assert
            Assert.AreEqual(expectedIssueTitle, actualIssue.Title);
            Assert.AreEqual(expectedIssueDescription, actualIssue.Description);
        }


        [Test]
        public async Task CreateIssue_Should_Return_False_And_Proper_ErrorMessage_If_Is_Passed_Invalid_CourseId()
        {
            //Arrange                      
            var issueService = serviceProvider.GetService<IIssueService>();
            int invalidCourseId = -1;
            bool expectedResult = false;
            string errorMessage = "Invalid course id.";

            //Act
            var actualResult = await issueService.CreateIssue(
                    "EEEEEEEE-6666-6666-6666-331431D13211",
                    invalidCourseId,
                    "testTitle",
                    "testDescription"
                );

            //Assert
            Assert.AreEqual(expectedResult, actualResult.Item1);
            Assert.AreEqual(errorMessage, actualResult.Item2);
        }


        [Test]
        public async Task DeleteIssue_Should_Delete_Issue()
        {
            //Arrange
            int issueId = 1;
            bool expectedExisting = false;
            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.DeleteIssue(issueId);
            bool isExists = sqliteDbContext.DbContext.Issues.Any(c => c.Title == "First Issue" && c.Id == 1);

            //Assert
            Assert.AreEqual(expectedExisting, isExists);
        }


        [Test]
        public async Task DeleteIssue_Should_Return_True_Without_ErrorMessage_If_Comemnt_Is_Deleted()
        {
            //Arrange
            int issueId = 1;
            bool expectedResult = true;
            string expectedErrorMessage = null;
            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.DeleteIssue(issueId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult.Item1);
            Assert.AreEqual(expectedErrorMessage, actualResult.Item2);
        }


        [Test]
        public async Task DeleteIssue_Should_Return_False_And_Correct_ErrorMessage_If_Invalid_Id_Is_Passed()
        {
            //Arrange
            int invalidIssueId = -1;
            bool expectedResult = false;
            string expectedErrorMessage = "Invalid issue id.";
            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.DeleteIssue(invalidIssueId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult.Item1);
            Assert.AreEqual(expectedErrorMessage, actualResult.Item2);
        }


        [Test]
        public async Task EditIssue_Should_Edit_Issue()
        {
            //Arrange
            string expectedIssueTitle = "Updated issue title";
            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.EditIssue(1, expectedIssueTitle, "Issue description");
            var issue = await sqliteDbContext.DbContext.Issues.FirstOrDefaultAsync(i => i.Id == 1);

            //Assert
            Assert.AreEqual(expectedIssueTitle, issue.Title);
        }


        [Test]
        public async Task EditIssue_Should_Return_True_Without_ErrorMessage_If_Issue_Is_Edited()
        {
            //Arrange
            int issueId = 1;
            bool expectedResult = true;
            string expectedErrorMessage = null;
            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.EditIssue(1, "Test title", "Issue description");

            //Assert
            Assert.AreEqual(expectedResult, actualResult.Item1);
            Assert.AreEqual(expectedErrorMessage, actualResult.Item2);
        }


        [Test]
        public async Task EditIssue_Should_Return_False_With_Correct_ErrorMessage_If_Is_Passed_Invalid_IssueId()
        {
            //Arrange
            int invalidIssueId = -1;
            bool expectedResult = false;
            string expectedErrorMessage = "Invalid issue id.";
            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.EditIssue(invalidIssueId, "Test title", "Issue description");

            //Assert
            Assert.AreEqual(expectedResult, actualResult.Item1);
            Assert.AreEqual(expectedErrorMessage, actualResult.Item2);
        }


        [Test]
        public async Task GetAllReportedIssues_Should_Get_All_Reported_Issues()
        {
            //Arrange
            List<AllIssuesServiceModel> allReportedIssues = new List<AllIssuesServiceModel>()
            {
                new AllIssuesServiceModel
                {
                    Title = "First Issue",
                    Description = "First Issue Description",
                },
                new AllIssuesServiceModel
                {
                   Title = "Second Issue",
                   Description = "Second Issue Description",
                }
            };

            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.GetAllReportedIssues();

            //Assert
            for (int i = 0; i < allReportedIssues.Count; i++)
            {
                Assert.AreEqual(allReportedIssues[i].Title, actualResult.ElementAt(i).Title);
                Assert.AreEqual(allReportedIssues[i].Description, actualResult.ElementAt(i).Description);
            }
        }


        [Test]
        public async Task GetAllReportedIssuesForCourse_Should_Get_All_Reported_Issues_For_Course()
        {
            //Arrange
            int courseId = 1;
            List<AllIssuesServiceModel> allReportedIssues = new List<AllIssuesServiceModel>()
            {
                new AllIssuesServiceModel
                {
                    Title = "First Issue",
                    Description = "First Issue Description",
                },
                new AllIssuesServiceModel
                {
                   Title = "Second Issue",
                   Description = "Second Issue Description",
                }
            };

            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.GetAllReportedIssuesForCourse(courseId);

            //Assert
            for (int i = 0; i < allReportedIssues.Count; i++)
            {
                Assert.AreEqual(allReportedIssues[i].Title, actualResult.ElementAt(i).Title);
                Assert.AreEqual(allReportedIssues[i].Description, actualResult.ElementAt(i).Description);
            }
        }


        [Test]
        public async Task GetMyReportedIssues_Should_Get_All_My_Reported_Issues()
        {
            //Arrange
            int courseId = 1;
            string userId = "EEEEEEEE-6666-6666-6666-331431D13211";

            List<AllIssuesServiceModel> allReportedIssues = new List<AllIssuesServiceModel>()
            {
                new AllIssuesServiceModel
                {
                    Title = "First Issue",
                    Description = "First Issue Description",
                },
                new AllIssuesServiceModel
                {
                   Title = "Second Issue",
                   Description = "Second Issue Description",
                }
            };

            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.GetMyReportedIssues(userId, courseId);

            //Assert
            for (int i = 0; i < allReportedIssues.Count; i++)
            {
                Assert.AreEqual(allReportedIssues[i].Title, actualResult.ElementAt(i).Title);
                Assert.AreEqual(allReportedIssues[i].Description, actualResult.ElementAt(i).Description);
            }
        }


        [Test]
        public async Task GetIssueDetails_Should_Return_Issue_Details()
        {
            //Arrange
            int issueId = 1;
            IssueDetailsServiceModel issueDetailsModel = new IssueDetailsServiceModel
            {
                CourseId = 1,
                Title = "First Issue",
                Description = "First Issue Description",
            };

            var issueService = serviceProvider.GetService<IIssueService>();

            //Act
            var actualResult = await issueService.GetIssueDetails(issueId);

            //Assert
            Assert.AreEqual(issueDetailsModel.CourseId, actualResult.CourseId);
            Assert.AreEqual(issueDetailsModel.Title, actualResult.Title);
            Assert.AreEqual(issueDetailsModel.Description, actualResult.Description);         
        }
    }
}
