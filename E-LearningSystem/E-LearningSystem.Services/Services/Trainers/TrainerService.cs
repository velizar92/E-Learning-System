namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Enums;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Data.Data.Models;

    using static E_LearningSystem.Infrastructure.Constants.IdentityConstants;

    public class TrainerService : ITrainerService
    {
        private readonly ELearningSystemDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<User> userManager;
       
        public TrainerService(
            ELearningSystemDbContext dbContext,
            IWebHostEnvironment webHostEnvironment,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;        
        }


        public async Task BecomeTrainer(User user, string fullName, IFormFile cvUrl)
        {
            string detailPath = Path.Combine(@"\assets\resources", cvUrl.FileName);
            using (var stream = new FileStream(webHostEnvironment.WebRootPath + detailPath, FileMode.Create))
            {
                await cvUrl.CopyToAsync(stream);
            }

            await userManager.RemoveFromRoleAsync(user, LearnerRole);
            await userManager.AddToRoleAsync(user, TrainerRole);

            dbContext.Entry(user).State = EntityState.Modified;

            var trainer = new Trainer
            {
                FullName = fullName,
                ProfileImageUrl = user.ProfileImageUrl,
                Rating = 0,
                CVUrl = cvUrl.FileName,
                Status = TrainerStatus.Pending,
                UserId = user.Id
            };

            await this.dbContext.Trainers.AddAsync(trainer);
            await this.dbContext.SaveChangesAsync();
        }


        public async Task<bool> CheckIfTrainerExists(string userId)
        {
            var isUserAlreadyTrainer = await this.dbContext
                .Trainers
                .AnyAsync(d => d.UserId == userId);

            return isUserAlreadyTrainer;
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


        public async Task<IEnumerable<AllTrainersServiceModel>> GetActiveTrainers()
        {
            var trainerUsers = await userManager.GetUsersInRoleAsync(TrainerRole);

            return dbContext
                        .Trainers
                        .Where(t => t.Status == TrainerStatus.Active)
                        .Select(t => new AllTrainersServiceModel
                        {
                            Id = t.Id,
                            FullName = t.FullName,
                            ProfileImageUrl = t.ProfileImageUrl,
                            Rating = t.Rating,
                            Status = t.Status
                        })
                        .ToList();
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
                            Rating = t.Rating,
                            Status = t.Status
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
                            Rating = t.Rating,
                            Status = t.Status
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

        public async Task<bool> VoteForTrainer(string userId, int trainerId)
        {
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);

            if (trainer == null)
            {
                return false;
            }

            if (await dbContext.Votes.FirstOrDefaultAsync(v => v.UserId == userId && v.TrainerId == trainerId) == null)
            {
                dbContext.Votes.Add(new Vote { UserId = userId, TrainerId = trainerId });

                if (trainer.Rating == null)
                {
                    trainer.Rating = 0;
                }

                trainer.Rating = trainer.Rating + 1;
            }
            else
            {
                return false;
            }

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
