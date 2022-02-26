namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Data.Enums;

    public class TrainerService : ITrainerService
    {
        private readonly ELearningSystemDbContext dbContext;

        public TrainerService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }


        public Task<int> CreateTrainer(string _firstName, string _lastName, string _userName, string _email, string _profileImageUrl, string _cvUrl, TrainerStatus _trainerStatus)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTrainer(int _trainerId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditTrainer(int _trainerId, string _firstName, string _lastName, string _userName, string _email, string _profileImageUrl, string _cvUrl, TrainerStatus _trainerStatus)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task<AllTrainersServiceModel>> GetAllTrainers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task<AllTrainersServiceModel>> GetTrainersByCourse(string _courseName)
        {
            throw new NotImplementedException();
        }
    }
}
