﻿namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Enums;

    public interface ITrainerService
    {
        Task<int> CreateTrainer(string firstName, string lastName, string userName,
            string email, string profileImageUrl, string cvUrl, TrainerStatus trainerStatus);

        Task<bool> EditTrainer(int trainerId, string fullName, string cvUrl, TrainerStatus trainerStatus);

        Task<bool> DeleteTrainer(int trainerId);

        Task<IEnumerable<AllTrainersServiceModel>> GetAllTrainers();

    }
}
