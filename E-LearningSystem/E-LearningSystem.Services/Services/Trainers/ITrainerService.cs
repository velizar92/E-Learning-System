namespace E_LearningSystem.Services.Services
{
    using Microsoft.AspNetCore.Http;
    using E_LearningSystem.Data.Enums;
    using E_LearningSystem.Data.Models;
   
    public interface ITrainerService
    {
        Task<int> CreateTrainer(string firstName, string lastName, string userName, string password,
            string email, string profileImageUrl, string cvUrl, TrainerStatus trainerStatus);

        Task<bool> EditTrainer(int trainerId, string fullName, string cvUrl, TrainerStatus trainerStatus);

        Task<bool> DeleteTrainer(int trainerId, string userId);

        Task<IEnumerable<AllTrainersServiceModel>> GetAllTrainers();

        Task<IEnumerable<AllTrainersServiceModel>> GetActiveTrainers();

        Task<IEnumerable<AllTrainersServiceModel>> GetTopTrainers();

        Task<int> GetTrainerIdByUserId(string userId);

        Task<Trainer> GetTrainerByTrainerId(int trainerId);

        Task<bool> VoteForTrainer(string userId, int trainerId);

        Task BecomeTrainer(User user, string fullName, IFormFile cvUrl);

        Task<bool> CheckIfTrainerExists(string userId);

        Task<bool> ApproveTrainer(int id);

    }
}
