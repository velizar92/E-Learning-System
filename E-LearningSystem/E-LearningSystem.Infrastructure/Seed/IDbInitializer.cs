namespace E_LearningSystem.Infrastructure.Seed
{
    using Microsoft.AspNetCore.Builder;

    public interface IDbInitializer
    {
        Task InitializeDatabase(IApplicationBuilder applicationBuilder);
    }
}
