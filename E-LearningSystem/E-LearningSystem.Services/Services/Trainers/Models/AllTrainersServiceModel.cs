namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Enums;
    public class AllTrainersServiceModel
    {

        public int Id { get; set; }

        public string UserId { get; set; }

        public string FullName { get; set; }

        public string ProfileImageUrl { get; set; }

        public int? Rating { get; set; }

        public TrainerStatus Status { get; set; }
    }
}