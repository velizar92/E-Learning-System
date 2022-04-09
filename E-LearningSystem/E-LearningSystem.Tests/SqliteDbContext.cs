using E_LearningSystem.Data.Data;
using E_LearningSystem.Data.Data.Models;
using E_LearningSystem.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace E_LearningSystem.Tests
{
    public class SqliteDbContext
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";

        private readonly SqliteConnection _connection;

        public readonly ELearningSystemDbContext DbContext;

        public SqliteDbContext()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<ELearningSystemDbContext>()
                     .UseSqlite(_connection)
                     .Options;
            DbContext = new ELearningSystemDbContext(options);
     
            DbContext.Database.EnsureCreated();
            SeedDb();
        }

        public void Dispose()
        {
            _connection.Close();
        }

        private void SeedDb()
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
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Id = 1,
                                Title = "First Issue",
                                Description = "First Issue Description",
                                UserId = "EEEEEEEE-6666-6666-6666-331431D13211"
                            },
                            new Issue
                            {
                                Id = 2,
                                Title = "Second Issue",
                                Description = "Second Issue Description",
                                UserId = "EEEEEEEE-6666-6666-6666-331431D13211"
                            },

                        },
                        CourseUsers = new List<CourseUser>
                        {
                            new CourseUser
                            {
                                UserId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5",
                                CourseId = 1
                            },
                             new CourseUser
                            {
                                UserId = "EEEEEEEE-6666-6666-6666-331431D13211",
                                CourseId = 1
                            },
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
                            },
                            new Lecture
                            {
                                Id = 3,
                                Name = "Lecture 3",
                                Description = "Lecture 3 description",
                            }
                        }
                    },
                }
            };

             DbContext.Users.AddRange(users);
             DbContext.CourseCategories.AddRange(courseCategories);
             DbContext.Trainers.Add(trainer);
             DbContext.SaveChanges();
        }
    }
}
