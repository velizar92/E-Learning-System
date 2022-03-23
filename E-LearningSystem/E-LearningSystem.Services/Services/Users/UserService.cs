namespace E_LearningSystem.Services.Services.Users
{
    using Microsoft.EntityFrameworkCore;
    using E_LearningSystem.Data.Data;

    public class UserService : IUserService
    {
        private readonly ELearningSystemDbContext dbContext;

        public UserService(ELearningSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CheckIfUserHasCourse(string userId, int courseId)
        {
            if (await dbContext.CourseUsers.FirstOrDefaultAsync(cu => cu.UserId == userId && cu.CourseId == courseId) != null)
            {
                return true;
            }

            return false;
        }
    }
}
