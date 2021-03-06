namespace E_LearningSystem.Tests
{
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Services.Services;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Linq;

    public class CommentServiceTests
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
                .AddSingleton<ICommentService, CommentService>()
                .BuildServiceProvider();
        }


        [Test]
        public async Task CreateComment_Should_Create_Comment()
        {
            //Arrange          
            string expectedCommentContent = "Test comment";
            var commentService = serviceProvider.GetService<ICommentService>();
            int lectureId = 2;

            //Act
            var creationResult = await commentService.CreateComment(lectureId, "EEEEEEEE-6666-6666-6666-331431D13211", expectedCommentContent);

            var actualComment =
                await sqliteDbContext.DbContext.Comments
                                .FirstOrDefaultAsync(c => c.UserId == "EEEEEEEE-6666-6666-6666-331431D13211"
                                                       && c.Content == expectedCommentContent
                                                       && c.LectureId == lectureId);
            //Assert
            Assert.AreEqual(
                expectedCommentContent,
                actualComment.Content);
        }


        [Test]
        public async Task CreateComment_Should_Return_True_If_Comment_Is_Created()
        {
            //Arrange          
            bool expectedResult = true;
            string testComment = "Test comment";
            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var creationResult = await commentService.CreateComment(1, "EEEEEEEE-6666-6666-6666-331431D13211", testComment);

            //Assert
            Assert.AreEqual(expectedResult, creationResult);
        }


        [Test]
        public async Task DeleteComment_Should_Delete_Comment()
        {
            //Arrange
            bool result = true;
            int lectureId = 1;
            (bool, int?) expectedResult = (result, lectureId);
            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var actualResult = await commentService.DeleteComment(1);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task DeleteComment_Should_Return_FalseNull_If_Invalid_CommentId_Is_Passed()
        {
            //Arrange
            bool result = false;
            (bool, int?) expectedResult = (result, null);
            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var actualResult = await commentService.DeleteComment(-1);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task CreateComment_Should_Return_False_If_Passed_LectureId_Is_Invalid()
        {
            //Arrange          
            bool expectedResult = false;
            string testComment = "Test comment";
            int invalidLectureId = -1;
            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var creationResult = await commentService.CreateComment(invalidLectureId, "EEEEEEEE-6666-6666-6666-331431D13211", testComment);

            //Assert
            Assert.AreEqual(expectedResult, creationResult);
        }



        [Test]
        public async Task GetCommentById_Should_Return_Correct_Comment()
        {
            //Arrange          
            string expectedCommentContent = "Lecture 1 comment";
            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var comment = await commentService.GetCommentById(1);

            //Assert
            Assert.AreEqual(expectedCommentContent, comment.Content);
        }


        [Test]
        public async Task GetCommentById_Should_Return_Null_If_Is_Passed_Invalid_CommentId()
        {
            //Arrange           
            string expectedCommentContent = null;
            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var comment = await commentService.GetCommentById(-1);

            //Assert
            Assert.AreEqual(expectedCommentContent, comment);
        }


        [Test]
        public async Task GetLectureComments_Should_Return_All_Lecture_Comments()
        {
            //Arrange
            int lectureId = 1;
            List<AllLectureCommentsServiceModel> expectedComments = new List<AllLectureCommentsServiceModel>()
            {
                  new AllLectureCommentsServiceModel
                    {
                        UserId = "EEEEEEEE-6666-6666-6666-331431D13211",
                        Content = "Lecture 1 comment",
                        LectureId = 1,
                    }
            };

            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var lectureComments = await commentService.GetLectureComments(lectureId);
            var lectureCommentsList = lectureComments.ToList();

            //Assert
            for (int i = 0; i < expectedComments.Count(); i++)
            {
                Assert.AreEqual(expectedComments[i].LectureId, lectureCommentsList[i].LectureId);
                Assert.AreEqual(expectedComments[i].UserId, lectureCommentsList[i].UserId);
                Assert.AreEqual(expectedComments[i].Content, lectureCommentsList[i].Content);
            }       
        }


        [Test]
        public async Task GetLectureComments_Should_Return_Empty_Collection_If_Is_Passed_Invalid_LectureId()
        {
            //Arrange
            int invalidLectureId = -1;
            List<AllLectureCommentsServiceModel> expectedComments = new List<AllLectureCommentsServiceModel>();          

            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var lectureComments = await commentService.GetLectureComments(invalidLectureId);

            //Assert
            Assert.AreEqual(expectedComments.Count, lectureComments.Count());
        }


        [Test]
        public async Task GetLectureComments_Should_Return_Empty_Collection_If_There_Are_No_Comments_For_The_Particular_Lecture()
        {
            //Arrange
            int lectureId = 3;
            List<AllLectureCommentsServiceModel> expectedComments = new List<AllLectureCommentsServiceModel>();

            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var lectureComments = await commentService.GetLectureComments(lectureId);

            //Assert
            Assert.AreEqual(expectedComments.Count, lectureComments.Count());
        }


    }
}

