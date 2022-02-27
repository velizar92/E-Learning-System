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

        public TrainerService(ELearningSystemDbContext _dbContext, UserManager<User> _userManager)
        {
            this.dbContext = _dbContext;
            this.userManager = _userManager;
        }


        public async Task<int> CreateTrainer(string _firstName, string _lastName, string _userName,
            string _email, string _profileImageUrl, string _cvUrl, TrainerStatus _trainerStatus)
        {
            var user = new User()
            {
                FirstName = _firstName,
                LastName = _lastName,
                UserName = _userName,
                Email = _email,
                ProfileImageUrl = _profileImageUrl,
            };

            await userManager.CreateAsync(user);

            var trainer = new Trainer()
            {
                FullName = _firstName + " " + _lastName,
                CVUrl = _cvUrl,
                UserId = user.Id,
                Status = _trainerStatus,
            };

            await dbContext.Trainers.AddAsync(trainer);
            await dbContext.SaveChangesAsync();

            return trainer.Id;
        }


        public async Task<bool> DeleteTrainer(int _trainerId)
        {
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == _trainerId);

            if (trainer == null)
            {
                return false;
            }

            dbContext.Trainers.Remove(trainer);
            await dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> EditTrainer(int _trainerId, string _fullName, string _cvUrl, TrainerStatus _trainerStatus)
        {
            var trainer = await dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == _trainerId);

            if (trainer == null)
            {
                return false;
            }

            trainer.FullName = _fullName;
            trainer.CVUrl = _cvUrl;
            trainer.Status = _trainerStatus;

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
