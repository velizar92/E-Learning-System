namespace E_LearningSystem.Services.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> ApproveTrainer(int id)
        {
            var trainer = await this.dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null)
            {
                return false;
            }

            trainer.Status = TrainerStatus.Active;

            await dbContext.SaveChangesAsync();
            return true;
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

            this.dbContext.Entry(user).State = EntityState.Modified;

            var trainer = new Trainer
            {
                FullName = fullName,
                ProfileImageUrl = user.ProfileImageUrl,
                Rating = 0,
                CVUrl = cvUrl.FileName,
                Status = TrainerStatus.Pending,
                UserId = user.Id
            };

            var userCourses = this.dbContext.CourseUsers.Where(u => u.UserId == user.Id).ToArray();
            this.dbContext.CourseUsers.RemoveRange(userCourses);

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

            await this.dbContext.Trainers.AddAsync(trainer);
            await this.dbContext.SaveChangesAsync();

            return trainer.Id;
        }


        
        public async Task<bool> DeleteTrainer(int trainerId, string userId)
        {
            var trainer = await this.dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);
            var user = await this.dbContext.Users.FirstOrDefaultAsync(t => t.Id == userId);

            if (trainer == null || user == null)
            {
                return false;
            }
       
            await userManager.RemoveFromRoleAsync(user, TrainerRole);
            await userManager.AddToRoleAsync(user, LearnerRole);

            this.dbContext.Trainers.Remove(trainer);
            await this.dbContext.SaveChangesAsync();
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

            await this.dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<AllTrainersServiceModel>> GetActiveTrainers()
        {
            var trainerUsers = await userManager.GetUsersInRoleAsync(TrainerRole);

            return this.dbContext
                        .Trainers
                        .Where(t => t.Status == TrainerStatus.Active)
                        .Select(t => new AllTrainersServiceModel
                        {
                            Id = t.Id,
                            UserId = t.UserId,
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

            return this.dbContext
                        .Trainers
                        .Select(t => new AllTrainersServiceModel
                        {
                            Id = t.Id,
                            UserId = t.UserId,
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

            return this.dbContext.Trainers
                        .OrderByDescending(t => t.Rating)
                        .Select(t => new AllTrainersServiceModel
                        {
                            Id = t.Id,
                            UserId = t.UserId,
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
            var trainer = await this.dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);

            return trainer;
        }


        public async Task<int> GetTrainerIdByUserId(string userId)
        {
            var trainer = await this.dbContext.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);

            if(trainer == null)
            {
                return -1;
            }

            return trainer.Id;
        }

        public async Task<bool> VoteForTrainer(string userId, int trainerId)
        {
            var trainer = await this.dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == trainerId);

            if (trainer == null)
            {
                return false;
            }

            if (await this.dbContext.Votes.FirstOrDefaultAsync(v => v.UserId == userId && v.TrainerId == trainerId) == null)
            {
                this.dbContext.Votes.Add(new Vote { UserId = userId, TrainerId = trainerId });

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

            await this.dbContext.SaveChangesAsync();
            return true;
        }
    }
}
