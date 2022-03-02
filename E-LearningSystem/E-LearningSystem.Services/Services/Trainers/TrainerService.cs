namespace E_LearningSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Enums;
    using E_LearningSystem.Data.Models;

    public class TrainerService : ITrainerService
    {
        private readonly ELearningSystemDbContext dbContext;
        private readonly UserManager<User> userManager;

        public TrainerService(ELearningSystemDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }


        public async Task<int> CreateTrainer(string firstName, string lastName, string userName,
            string email, string profileImageUrl, string cvUrl, TrainerStatus trainerStatus)
        {
            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                ProfileImageUrl = profileImageUrl,
            };

            await userManager.CreateAsync(user);

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
            var trainerUsers = await userManager.GetUsersInRoleAsync("Trainer");

            return trainerUsers
                        .Select(t => new AllTrainersServiceModel
                        {
                            FullName = t.FirstName + " " + t.LastName,
                            ProfileImageUrl = t.ProfileImageUrl
                        })
                        .ToList();
        }

    }
}
