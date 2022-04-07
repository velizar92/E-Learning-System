namespace E_LearningSystem.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using System.Threading.Tasks; 
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Data.Data.Models;

    public class CommentServiceTests : SqliteDbContext
    {
      
        [SetUp]
        public async Task Setup()
        {
            await SeedDbAsync();
        }


        [Test]
        public async Task Test()
        {
            var commentService = new CommentService(this.DbContext);
            var commentData = await commentService.GetCommentById(1);
            Assert.AreEqual("Lecture 1 comment", commentData.Content);
        }


        private async Task SeedDbAsync()
        {
            var user = new User
            {
                Id = "EC90AD4D-7C94-4BAC-B04C-331431D132D5",
                FirstName = "Test",
                LastName = "Testov",
                UserName = "test@example.com",
                NormalizedUserName = "TEST@EXAMPLE.COM",
                Email = "test@example.com",
                ProfileImageUrl = "trainer-1.jpg"
            };

            List<CourseCategory> courseCategories = new List<CourseCategory>()
            {
                new CourseCategory()
                {
                    Id = 1,
                    Name = "Programming"
                },
                new CourseCategory()
                {
                    Id = 2,
                    Name = "Networking"
                },
                new CourseCategory()
                {
                    Id = 3,
                    Name = "Algorithms"
                },
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
                                         UserId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5"
                                    }
                                }
                            }
                        }
                    },
                }
            };

            await DbContext.Users.AddAsync(user);
            await DbContext.CourseCategories.AddRangeAsync(courseCategories);
            await DbContext.Trainers.AddAsync(trainer);
            await DbContext.SaveChangesAsync();
        }
    }
}
