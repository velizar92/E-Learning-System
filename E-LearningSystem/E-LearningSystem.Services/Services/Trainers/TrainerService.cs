namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Enums;
    using E_LearningSystem.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;

    public class TrainerService : ITrainerService
    {
        private readonly ELearningSystemDbContext dbContext;
        private readonly UserManager<User> userManager;

        public TrainerService(ELearningSystemDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }


        [Authorize(Roles = AdminRole)]
        public async Task<int> CreateTrainer(string firstName, string lastName, string userName,
            string password, string email, string profileImageUrl, string cvUrl, TrainerStatus trainerStatus)
        {
            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                ProfileImageUrl = profileImageUrl,
            };

            await userManager.CreateAsync(user, password);

            var trainer = new Trainer()
            {
                FullName = firstName + " " + lastName,
                CVUrl = cvUrl,
                UserId = user.Id,
                Status = trainerStatus,
            };

            await dbContext.Trainers.AddAsync(trainer);
            await dbContext.SaveChangesAsync();

            return trainer.Id;
        }


        [Authorize(Roles = AdminRole)]
        public async Task<bool> DeleteTrainer(int trainerId)
        {
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);

            if (trainer == null)
            {
                return false;
            }

            dbContext.Trainers.Remove(trainer);
            await dbContext.SaveChangesAsync();
            return true;
        }


        [Authorize(Roles = AdminRole)]
        public async Task<bool> EditTrainer(int trainerId, string fullName, string cvUrl, TrainerStatus trainerStatus)
        {
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);

            if (trainer == null)
            {
                return false;
            }

            trainer.FullName = fullName;
            trainer.CVUrl = cvUrl;
            trainer.Status = trainerStatus;

            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<AllTrainersServiceModel>> GetAllTrainers()
        {
            var trainerUsers = await userManager.GetUsersInRoleAsync(TrainerRole);

            return dbContext
                        .Trainers
                        .Select(t => new AllTrainersServiceModel
                        {
                            Id = t.Id,
                            FullName = t.FullName,
                            ProfileImageUrl = t.ProfileImageUrl,
                            Rating = t.Rating
                        })
                        .ToList();
        }


        public async Task<IEnumerable<AllTrainersServiceModel>> GetTopTrainers()
        {
            var trainerUsers = await userManager.GetUsersInRoleAsync(TrainerRole);

            return dbContext.Trainers
                        .OrderByDescending(t => t.Rating)
                        .Select(t => new AllTrainersServiceModel
                        {
                            Id = t.Id,
                            FullName = t.FullName,
                            ProfileImageUrl = t.ProfileImageUrl,
                            Rating = t.Rating
                        })
                        .Take(3)
                        .ToList();
        }


        public async Task<Trainer> GetTrainerByTrainerId(int trainerId)
        {
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);

            return trainer;
        }


        public async Task<int> GetTrainerIdByUserId(string userId)
        {
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);

            return trainer.Id;
        }

        public async Task<bool> VoteForTrainer(int trainerId)
        {
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);

            if(trainer == null)
            {
                return false;
            }
            
            if (trainer.Rating == null)
            {
                trainer.Rating = 0;
            }

            trainer.Rating = trainer.Rating + 1;

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
