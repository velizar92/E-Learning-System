namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Enums;
    using E_LearningSystem.Data.Models;
   
    public interface ITrainerService
    {
        Task<int> CreateTrainer(User user, string firstName, string lastName, string email,
            string password, string profileImageUrl, string cvUrl);

        Task<bool> EditTrainer(int trainerId, string fullName, string cvUrl, TrainerStatus trainerStatus);

        Task<bool> DeleteTrainer(int trainerId, string userId);

        Task<IEnumerable<AllTrainersServiceModel>> GetAllTrainers();

        Task<IEnumerable<AllTrainersServiceModel>> GetActiveTrainers();

        Task<IEnumerable<AllTrainersServiceModel>> GetTopTrainers();

        Task<int> GetTrainerIdByUserId(string userId);

        Task<Trainer> GetTrainerByTrainerId(int trainerId);

        Task<bool> VoteForTrainer(string userId, int trainerId);

        Task BecomeTrainer(User user, string fullName, string cvUrl);

        Task<bool> CheckIfTrainerExists(string userId);

        Task<bool> ApproveTrainer(int id);

    }
}
