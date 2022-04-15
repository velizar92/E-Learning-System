namespace E_LearningSystem.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using E_LearningSystem.Data.Enums;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class TrainerServiceTests
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
                .AddSingleton<ITrainerService, TrainerService>()
                .BuildServiceProvider();
        }

        [Test]
        public async Task CreateTrainer_Should_Create_Trainer()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();

            var user = new User
            {
                Id = "EEEEEEEE-6666-6666-6666-331431D13288",
                FirstName = "Ivan",
                LastName = "Tihomirov",
                UserName = "ivan@example.com",
                Email = "ivan@example.com",
                ProfileImageUrl = "testImageUrl"
            };

            await sqliteDbContext.DbContext.Users.AddAsync(user);

            Trainer expectedTrainer = new Trainer()
            {
                FullName = "Ivan Tihomirov",
                CVUrl = "testCvUrl",
                Status = TrainerStatus.Active,
                ProfileImageUrl = "testImageUrl",
                Rating = 0
            };

            //Act
            await trainerService.CreateTrainer
                (
                user, "Ivan", "Tihomirov", "ivan@example.com",
                "test123", "testImageUrl", "testCvUrl"
                );

            var searchedTrainer = await sqliteDbContext
                                    .DbContext
                                    .Trainers
                                    .FirstOrDefaultAsync
                                        (t => t.ProfileImageUrl == "testImageUrl" &&
                                        t.CVUrl == "testCvUrl");

            //Assert
            Assert.AreEqual(expectedTrainer.FullName, searchedTrainer.FullName);
            Assert.AreEqual(expectedTrainer.CVUrl, searchedTrainer.CVUrl);
            Assert.AreEqual(expectedTrainer.ProfileImageUrl, searchedTrainer.ProfileImageUrl);
            Assert.AreEqual(expectedTrainer.Status, searchedTrainer.Status);
            Assert.AreEqual(expectedTrainer.Rating, searchedTrainer.Rating);
        }


        [Test]
        public async Task EditTrainer_Should_Edit_Trainer()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();

            Trainer expectedTrainer = new Trainer
            {
                FullName = "Test Updatov",
                CVUrl = "UpdateCVUrl",
                Status = TrainerStatus.Active
            };

            //Act
            await trainerService.EditTrainer(1, "Test Updatov", "UpdateCVUrl", TrainerStatus.Active);

            var editedTrainer = await sqliteDbContext.DbContext.Trainers.FirstOrDefaultAsync(t => t.Id == 1);

            //Assert
            Assert.AreEqual(expectedTrainer.FullName, editedTrainer.FullName);
            Assert.AreEqual(expectedTrainer.CVUrl, editedTrainer.CVUrl);
            Assert.AreEqual(expectedTrainer.Status, editedTrainer.Status);
        }

        [Test]
        public async Task EditTrainer_Should_Return_False_If_Is_Passed_Invalid_TrainerId()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            bool expectedResult = false;

            //Act
            var actualResult = await trainerService.EditTrainer(-1, "Test Updatov", "UpdateCVUrl", TrainerStatus.Active);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);

        }

        [Test]
        public async Task GetTrainerByTrainerId_Should_Return_Correct_Trainer_By_TrainerId()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            int trainerId = 1;
            Trainer expectedTrainer = new Trainer
            {
                FullName = "Test Testov",
                UserId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5"
            };

            //Act
            var actualTrainer = await trainerService.GetTrainerByTrainerId(trainerId);

            //Assert
            Assert.AreEqual(expectedTrainer.FullName, actualTrainer.FullName);
            Assert.AreEqual(expectedTrainer.UserId, actualTrainer.UserId);
        }


        [Test]
        public async Task GetTrainerIdByUserId_Should_Return_Correct_TrainerId_By_UserId()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string userId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5";
            int expectedTrainerId = 1;

            //Act
            var actualTrainerId = await trainerService.GetTrainerIdByUserId(userId);

            //Assert
            Assert.AreEqual(expectedTrainerId, actualTrainerId);
        }

        [Test]
        public async Task GetTrainerIdByUserId_Should_MinusOne_If_Is_Passed_Invalid_UserId()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string invalidUserId = "EC90AD4D-7C94-4BAC-B04C-331431D15665";
            int expectedTrainerId = -1;

            //Act
            var actualTrainerId = await trainerService.GetTrainerIdByUserId(invalidUserId);

            //Assert
            Assert.AreEqual(expectedTrainerId, actualTrainerId);
        }


        [Test]
        public async Task GetActiveTrainers_Should_Return_All_Active_Trainers()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            IEnumerable<AllTrainersServiceModel> expectedTrainers = new List<AllTrainersServiceModel>
            {
               new AllTrainersServiceModel
               {
                    Id = 1,
                    UserId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5",
                    FullName = "Test Testov",
                    ProfileImageUrl = "TestUrl",
                    Rating = 0,
                    Status = TrainerStatus.Active
               }
            };

            //Act
            var actualTrainers = await trainerService.GetActiveTrainers();

            //Assert
            for (int i = 0; i < expectedTrainers.Count(); i++)
            {
                Assert.AreEqual(expectedTrainers.ElementAt(i).Id, actualTrainers.ElementAt(i).Id);
                Assert.AreEqual(expectedTrainers.ElementAt(i).UserId, actualTrainers.ElementAt(i).UserId);
                Assert.AreEqual(expectedTrainers.ElementAt(i).FullName, actualTrainers.ElementAt(i).FullName);
                Assert.AreEqual(expectedTrainers.ElementAt(i).ProfileImageUrl, actualTrainers.ElementAt(i).ProfileImageUrl);
                Assert.AreEqual(expectedTrainers.ElementAt(i).Rating, actualTrainers.ElementAt(i).Rating);
                Assert.AreEqual(expectedTrainers.ElementAt(i).Status, actualTrainers.ElementAt(i).Status);       
            }        
        }


        [Test]
        public async Task GetAllTrainers_Should_Return_All_Trainers()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            IEnumerable<AllTrainersServiceModel> expectedTrainers = new List<AllTrainersServiceModel>
            {
               new AllTrainersServiceModel
               {
                    Id = 1,
                    UserId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5",
                    FullName = "Test Testov",
                    ProfileImageUrl = "TestUrl",
                    Rating = 0,
                    Status = TrainerStatus.Active
               }
            };

            //Act
            var actualTrainers = await trainerService.GetAllTrainers();

            //Assert
            for (int i = 0; i < expectedTrainers.Count(); i++)
            {
                Assert.AreEqual(expectedTrainers.ElementAt(i).Id, actualTrainers.ElementAt(i).Id);
                Assert.AreEqual(expectedTrainers.ElementAt(i).UserId, actualTrainers.ElementAt(i).UserId);
                Assert.AreEqual(expectedTrainers.ElementAt(i).FullName, actualTrainers.ElementAt(i).FullName);
                Assert.AreEqual(expectedTrainers.ElementAt(i).ProfileImageUrl, actualTrainers.ElementAt(i).ProfileImageUrl);
                Assert.AreEqual(expectedTrainers.ElementAt(i).Rating, actualTrainers.ElementAt(i).Rating);
                Assert.AreEqual(expectedTrainers.ElementAt(i).Status, actualTrainers.ElementAt(i).Status);
            }
        }


        [Test]
        public async Task VoteForTrainer_Should_Increase_Rating_Of_Trainer_By_One()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string userId = "TTTTEEEE-6666-6666-6666-331431D17777";
            int trainerId = 1;
            double expectedRating = 1;

            //Act
            await trainerService.VoteForTrainer(userId, trainerId);

            var searchedTrainer = await sqliteDbContext.DbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);

            //Assert
            Assert.AreEqual(expectedRating, searchedTrainer.Rating);
        }


        [Test]
        public async Task VoteForTrainer_Should_Return_False_If_Is_Passed_Invalid_TrainerId()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string userId = "EEEEEEEE-6666-6666-6666-331431D13211";
            int invalidTrainerId = -1;
            bool expectedResult = false;

            //Act
            bool actualResult = await trainerService.VoteForTrainer(userId, invalidTrainerId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task VoteForTrainer_Should_Return_False_If_User_Already_Voted_For_The_Same_Trainer()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string userId = "EEEEEEEE-6666-6666-6666-331431D13211";
            int trainerId = 1;
            bool expectedResult = false;

            //Act
            bool actualResult = await trainerService.VoteForTrainer(userId, trainerId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task CheckIfTrainerExists_Should_Return_True_If_Trainer_Exists()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string userId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5";        
            bool expectedResult = true;

            //Act
            bool actualResult = await trainerService.CheckIfTrainerExists(userId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task CheckIfTrainerExists_Should_Return_False_If_Trainer_Not_Exists()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string invalidUserId = "EC90AD4D-7C94-4BAC-B04C-331431D13211";
            bool expectedResult = false;

            //Act
            bool actualResult = await trainerService.CheckIfTrainerExists(invalidUserId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task DeleteTrainer_Should_Delete_Trainer()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string userId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5";
            int trainerId = 1;
        
            //Act
            await trainerService.DeleteTrainer(trainerId, userId);
            var searchedTrainer = await sqliteDbContext.DbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);

            //Assert
            Assert.AreEqual(null, searchedTrainer);
        }


        [Test]
        public async Task DeleteTrainer_Should_Return_True_If_Trainer_Is_Deleted()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string userId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5";
            int trainerId = 1;
            bool expectedResult = true;

            //Act
            bool actualResult = await trainerService.DeleteTrainer(trainerId, userId);
            
            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task DeleteTrainer_Should_Return_False_If_TrainerId_Is_Invalid()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string userId = "EC90AD4D-7C94-4BAC-B04C-331431D132D5";
            int invalidTrainerId = -1;
            bool expectedResult = false;

            //Act
            bool actualResult = await trainerService.DeleteTrainer(invalidTrainerId, userId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task DeleteTrainer_Should_Return_False_If_UserId_Is_Invalid()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string invalidUserId = "ER90AD4D-7C94-4BAC-B04C-331431D132D5";
            int trainerId = 1;
            bool expectedResult = false;

            //Act
            bool actualResult = await trainerService.DeleteTrainer(trainerId, invalidUserId);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public async Task BecomeTrainer_Should_Create_Temp_Trainer_In_Pending_Status()
        {
            //Arrange
            var trainerService = serviceProvider.GetService<ITrainerService>();
            string userId = "EEEEEEEE-6666-6666-6666-331431D13211";

            var expectedTrainer = new Trainer
            {
                FullName = "Ivan Ivanov",
                ProfileImageUrl = "ivan.jpg",
                Rating = 0,
                CVUrl = "testCVUrl.pdf",
                Status = TrainerStatus.Pending,
                UserId = userId
            };

            //Act
            var user = await sqliteDbContext.DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            await trainerService.BecomeTrainer(user, "Ivan Ivanov", "testCVUrl.pdf");

            var searchedTrainer = await sqliteDbContext.DbContext.Trainers.FirstOrDefaultAsync(
                t => t.FullName == "Ivan Ivanov" && t.CVUrl == "testCVUrl.pdf" && t.Status == TrainerStatus.Pending);

            //Assert
            Assert.AreEqual(expectedTrainer.FullName, searchedTrainer.FullName);
            Assert.AreEqual(expectedTrainer.ProfileImageUrl, searchedTrainer.ProfileImageUrl);
            Assert.AreEqual(expectedTrainer.Rating, searchedTrainer.Rating);
            Assert.AreEqual(expectedTrainer.CVUrl, searchedTrainer.CVUrl);
            Assert.AreEqual(expectedTrainer.Status, searchedTrainer.Status);
            Assert.AreEqual(expectedTrainer.UserId, searchedTrainer.UserId);
        }
    }
}
