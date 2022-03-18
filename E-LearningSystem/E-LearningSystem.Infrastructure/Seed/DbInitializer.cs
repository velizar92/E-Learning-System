namespace E_LearningSystem.Infrastructure.Seed
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using E_LearningSystem.Data.Data;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Data.Enums;

    using static Constants.IdentityConstants;


    public class DbInitializer : IDbInitializer
    {

        private static List<User> trainerUsers = new List<User>()
            {
               new User
                    {
                        FirstName = "Mario",
                        LastName = "Gerasimov",
                        UserName ="mario@example.com",
                        NormalizedUserName = "MARIO@EXAMPLE.COM",
                        Email = "mario@example.com",
                        ProfileImageUrl = "trainer-1.jpg"
                    },
               new User
                    {
                        FirstName = "Emil",
                        LastName = "Elenkov",
                        UserName = "emil@example.com",
                        NormalizedUserName = "EMIL@EXAMPLE.COM",
                        Email = "emil@example.com",
                        ProfileImageUrl = "trainer-3.jpg"
                    },
               new User
                    {
                        FirstName = "Victoria",
                        LastName = "Petkova",
                        UserName = "victoria@example.com",
                        NormalizedUserName = "VICTORIA@EXAMPLE.COM",
                        Email = "victoria@example.com",
                        ProfileImageUrl = "trainer-2.jpg"
                    },
               new User
                    {
                        FirstName = "Denislav",
                        LastName = "Petrov",
                        UserName = "denislav@example.com",
                        NormalizedUserName = "DENISLAV@EXAMPLE.COM",
                        Email = "denislav@example.com",
                        ProfileImageUrl = "trainer-6.jpg"
                    },
               new User
                    {
                        FirstName = "Natalia",
                        LastName = "Dimitrova",
                        UserName = "natalia@example.com",
                        NormalizedUserName = "NATALIA@EXAMPLE.COM",
                        Email = "natalia@example.com",
                        ProfileImageUrl = "trainer-5.jpg"
                    },
                new User
                    {
                        FirstName = "Silvia",
                        LastName = "Stancheva",
                        UserName = "silvia@example.com",
                        NormalizedUserName = "SILVIA@EXAMPLE.COM",
                        Email = "silvia@example.com",
                        ProfileImageUrl = "trainer-4.jpg"
                    }
             };


        public async Task InitializeDatabase(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();

                var context = MigrateDatabase(services);

                await SeedCourseCategories(services);
                await SeedResourceTypes(services);
                await SeedRoles(services);

                if (userManager.Users.Any() == false)
                {
                    await SeedAdminUsers(services);
                    await SeedTrainerUsers(services);
                }
            }
        }


        private async Task<ELearningSystemDbContext> MigrateDatabase(IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<ELearningSystemDbContext>();

            await dbContext.Database.MigrateAsync();

            return dbContext;
        }


        private async Task SeedAdminUsers(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var dbContext = services.GetRequiredService<ELearningSystemDbContext>();

            User adminUser = new User
            {
                FirstName = "Velizar",
                LastName = "Gerasimov",
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                ProfileImageUrl = "admin_avatar.jpg"
            };

            IdentityResult result = await userManager.CreateAsync(adminUser, "test123");

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, AdminRole).Wait();
            }

            await dbContext.SaveChangesAsync();
        }


        private async static Task SeedTrainerUsers(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var dbContext = services.GetRequiredService<ELearningSystemDbContext>();


            await userManager.CreateAsync(trainerUsers[0], "Test1111");
            await userManager.AddToRoleAsync(trainerUsers[0], TrainerRole);

            await userManager.CreateAsync(trainerUsers[1], "Test2222");
            await userManager.AddToRoleAsync(trainerUsers[1], TrainerRole);

            await userManager.CreateAsync(trainerUsers[2], "Test3333");
            await userManager.AddToRoleAsync(trainerUsers[2], TrainerRole);

            await userManager.CreateAsync(trainerUsers[3], "Test4444");
            await userManager.AddToRoleAsync(trainerUsers[3], TrainerRole);

            await userManager.CreateAsync(trainerUsers[4], "Test5555");
            await userManager.AddToRoleAsync(trainerUsers[4], TrainerRole);

            await userManager.CreateAsync(trainerUsers[5], "Test6666");
            await userManager.AddToRoleAsync(trainerUsers[5], TrainerRole);


            //======================Seed trainers=====================
            var trainer1 = new Trainer
            {
                UserId = trainerUsers[0].Id,
                FullName = trainerUsers[0].FirstName + " " + trainerUsers[0].LastName,
                ProfileImageUrl = trainerUsers[0].ProfileImageUrl,
                Status = TrainerStatus.Active,
                CVUrl = "trainer1.pdf"
            };

            var trainer2 = new Trainer
            {
                UserId = trainerUsers[1].Id,
                FullName = trainerUsers[1].FirstName + " " + trainerUsers[1].LastName,
                ProfileImageUrl = trainerUsers[1].ProfileImageUrl,
                Status = TrainerStatus.Active,
                CVUrl = "trainer2.pdf"
            };

            var trainer3 = new Trainer
            {
                UserId = trainerUsers[2].Id,
                FullName = trainerUsers[2].FirstName + " " + trainerUsers[2].LastName,
                ProfileImageUrl = trainerUsers[2].ProfileImageUrl,
                Status = TrainerStatus.Active,
                CVUrl = "trainer3.pdf"
            };

            var trainer4 = new Trainer
            {
                UserId = trainerUsers[3].Id,
                FullName = trainerUsers[3].FirstName + " " + trainerUsers[3].LastName,
                ProfileImageUrl = trainerUsers[3].ProfileImageUrl,
                Status = TrainerStatus.Active,
                CVUrl = "trainer4.pdf"
            };

            var trainer5 = new Trainer
            {
                UserId = trainerUsers[4].Id,
                FullName = trainerUsers[4].FirstName + " " + trainerUsers[4].LastName,
                ProfileImageUrl = trainerUsers[4].ProfileImageUrl,
                Status = TrainerStatus.Active,
                CVUrl = "trainer5.pdf"
            };

            var trainer6 = new Trainer
            {
                UserId = trainerUsers[5].Id,
                FullName = trainerUsers[5].FirstName + " " + trainerUsers[5].LastName,
                ProfileImageUrl = trainerUsers[5].ProfileImageUrl,
                Status = TrainerStatus.Active,
                CVUrl = "trainer6.pdf"
            };

            dbContext.Trainers.AddRange(trainer1, trainer2, trainer3, trainer4, trainer5, trainer6);
            await dbContext.SaveChangesAsync();
        }



        private async Task SeedRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = services.GetRequiredService<ELearningSystemDbContext>();

            if (!roleManager.RoleExistsAsync(AdminRole).Result)
            {
                IdentityRole adminRole = new IdentityRole();
                adminRole.Name = AdminRole;
                IdentityResult roleResult = await roleManager.CreateAsync(adminRole);
            }

            if (!roleManager.RoleExistsAsync(TrainerRole).Result)
            {
                IdentityRole trainerRole = new IdentityRole();
                trainerRole.Name = TrainerRole;
                IdentityResult roleResult = await roleManager.CreateAsync(trainerRole);
            }

            if (!roleManager.RoleExistsAsync(LearnerRole).Result)
            {
                IdentityRole learnerRole = new IdentityRole();
                learnerRole.Name = LearnerRole;
                IdentityResult roleResult = await roleManager.CreateAsync(learnerRole);
            }

            await dbContext.SaveChangesAsync();
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
