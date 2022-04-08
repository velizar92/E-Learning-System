namespace E_LearningSystem.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Data.Data.Models;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class CommentServiceTests : SqliteDbContext
    {
        private ServiceProvider serviceProvider;       

        [SetUp]
        public async Task Setup()
        {
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => DbContext)          
                .AddSingleton<ICommentService, CommentService>()
                .BuildServiceProvider();

            if (!DbContext.Users.Any())
            {
                await SeedDbAsync();
            }                   
        }


        [Test]
        public async Task CreateComment_Must_Create_Comment()
        {
            //Arrange          
            string expectedCommentContent = "Test comment";
            var commentService = serviceProvider.GetService<ICommentService>();
            int lectureId = 2;

            //Act
            var creationResult = await commentService.CreateComment(lectureId, "EEEEEEEE-6666-6666-6666-331431D13211", expectedCommentContent);

            var actualComment = 
                await DbContext.Comments
                                .FirstOrDefaultAsync(c => c.UserId == "EEEEEEEE-6666-6666-6666-331431D13211" 
                                                       && c.Content == expectedCommentContent
                                                       && c.LectureId == lectureId);
            //Assert
            Assert.AreEqual(
                expectedCommentContent,
                actualComment.Content);
        }


        [Test]
        public async Task CreateComment_Must_Return_True_If_Comment_Is_Created()
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
        public async Task CreateComment_Must_Return_False_If_Passed_LectureId_Is_Invalid()
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
        public async Task GetCommentById_Must_Return_Correct_Comment()
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
        public async Task GetCommentById_Must_Return_Null_If_Is_Passed_Invalid_CommentId()
        {
            //Arrange           
            string expectedCommentContent = null;
            var commentService = serviceProvider.GetService<ICommentService>();

            //Act
            var comment = await commentService.GetCommentById(-1);

            //Assert
            Assert.AreEqual(expectedCommentContent, comment);
        }


        private async Task SeedDbAsync()
        {
            List<User> users = new List<User>()
            {
                 new User
                    {
                        Id = "EC90AD4D-7C94-4BAC-B04C-331431D132D5",
                        FirstName = "Test",
                        LastName = "Testov",
                        UserName = "test@example.com",
                        NormalizedUserName = "TEST@EXAMPLE.COM",
                        Email = "test@example.com",
                        ProfileImageUrl = "trainer-1.jpg"
                    },
                  new User
                    {
                        Id = "EEEEEEEE-6666-6666-6666-331431D13211",
                        FirstName = "Ivan",
                        LastName = "Ivanov",
                        UserName = "ivan@example.com",
                        NormalizedUserName = "IVAN@EXAMPLE.COM",
                        Email = "ivan@example.com",
                        ProfileImageUrl = "ivan.jpg"
                    },
            };


            List<CourseCategory> courseCategories = new List<CourseCategory>()
            {
                new CourseCategory() { Id = 1,   Name = "Programming"},
                new CourseCategory() { Id = 2,   Name = "Networking"},
                new CourseCategory() { Id = 3,   Name = "Algorithms"},
            };


            var trainer = new Trainer()
            {
                Id = 1,
                FullName = "Test Testov",
                CVUrl = "testUrl",
                ProfileImageUrl = "TestUrl",
                UserId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5",
                Courses = new List<Course>
                {
                    new Course()
                    {
                        Id = 1,
                        Name = "C# Basics",
                        Description = "C# basics description",
                        Price = 100.00,
                        ImageUrl = "testUrl",
                        CourseCategoryId = 1,
                        CourseUsers = new List<CourseUser>
                        {
                            new CourseUser
                            {
                                UserId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5",
                                CourseId = 1
                            }
                        },
                        Lectures = new List<Lecture>
                        {
                            new Lecture
                            {
                                Id = 1,
                                Name = "Lecture 1",
                                Description = "Lecture 1 description",
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                         Id = 1,
                                         Content = "Lecture 1 comment",
                                         UserId = "EEEEEEEE-6666-6666-6666-331431D13211"
                                    }
                                }
                            },
                            new Lecture
                            {
                                Id = 2,
                                Name = "Lecture 2",
                                Description = "Lecture 2 description",
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {                
                                         Id = 2,
                                         Content = "Lecture 2 comment",
                                         UserId = "EEEEEEEE-6666-6666-6666-331431D13211"
                                    }
                                }
                            }
                        }
                    },
                }
            };

            await DbContext.Users.AddRangeAsync(users);
            await DbContext.CourseCategories.AddRangeAsync(courseCategories);
            await DbContext.Trainers.AddAsync(trainer);
            await DbContext.SaveChangesAsync();
            
        }
    }
}
