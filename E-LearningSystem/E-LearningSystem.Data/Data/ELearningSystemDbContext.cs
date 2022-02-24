namespace E_LearningSystem.Data.Data
{
    using E_LearningSystem.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using E_LearningSystem.Data.Data.Models;

    public class ELearningSystemDbContext : IdentityDbContext<IdentityUser>
    {

        public ELearningSystemDbContext(DbContextOptions<ELearningSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Resource>()
              .HasOne(p => p.Lecture)
              .WithMany(b => b.Resources)
              .OnDelete(DeleteBehavior.Restrict);

            builder
              .Entity<Trainer>()
              .HasOne<User>()
              .WithOne()
              .HasForeignKey<Trainer>(d => d.UserId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
