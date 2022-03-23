namespace E_LearningSystem.Services.Services.Users
{
    public interface IUserService
    {
        Task<bool> CheckIfUserHasCourse(string userId, int courseId);
    }
}
