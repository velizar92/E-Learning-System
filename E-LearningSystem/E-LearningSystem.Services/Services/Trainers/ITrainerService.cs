﻿namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data.Enums;

    public interface ITrainerService
    {
        Task<int> CreateTrainer(string _firstName, string _lastName, string _userName,
            string _email, string _profileImageUrl, string _cvUrl, TrainerStatus _trainerStatus);

        Task<bool> EditTrainer(int _trainerId, string _firstName, string _lastName, string _userName,
            string _email, string _profileImageUrl, string _cvUrl, TrainerStatus _trainerStatus);

        Task<bool> DeleteTrainer(int _trainerId);

        IEnumerable<Task<AllTrainersServiceModel>> GetAllTrainers();

        IEnumerable<Task<AllTrainersServiceModel>> GetTrainersByCourse(string _courseName);
    }
}