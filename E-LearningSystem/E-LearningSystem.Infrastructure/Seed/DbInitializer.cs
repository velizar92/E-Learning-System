namespace E_LearningSystem.Infrastructure.Seed
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    
    public class DbInitializer : IDbInitializer
    {
        public async Task InitializeDatabase(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var context = MigrateDatabase(services);         

                await SeedCourseCategories(services);
                await SeedResourceTypes(services);              
            }
        }


        private async Task<ELearningSystemDbContext> MigrateDatabase(IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<ELearningSystemDbContext>();

            await dbContext.Database.MigrateAsync();

            return dbContext;
        }


        private async Task SeedCourseCategories(IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<ELearningSystemDbContext>();
            List<CourseCategory> courseCategories = new List<CourseCategory>
            {
                new CourseCategory{ Name = "Programming" },
                new CourseCategory{ Name = "Networking" },
                new CourseCategory{ Name = "Algorithms" },
            };

            if (!dbContext.CourseCategories.Any())
            {
                await dbContext.CourseCategories.AddRangeAsync(courseCategories);
                await dbContext.SaveChangesAsync();
            }
        }


        private async Task SeedResourceTypes(IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<ELearningSystemDbContext>();
            List<ResourceType> resourceTypes = new List<ResourceType>
            {
                new ResourceType{ Name = "PPT Presentation" },
                new ResourceType{ Name = "Video MP4" },
                new ResourceType{ Name = "PDF Document" },
            };

            if (!dbContext.ResourceTypes.Any())
            {
                await dbContext.ResourceTypes.AddRangeAsync(resourceTypes);
                await dbContext.SaveChangesAsync();
            }
        }


    }
}
