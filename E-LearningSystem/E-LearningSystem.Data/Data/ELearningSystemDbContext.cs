namespace E_LearningSystem.Data.Data
{
    using System.Threading;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using E_LearningSystem.Data.Models;

    public class ELearningSystemDbContext : IdentityDbContext<User>
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
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
      
      
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

            builder
             .Entity<ShoppingCart>()
             .HasOne<User>()
             .WithOne()
             .HasForeignKey<ShoppingCart>(d => d.UserId)
             .OnDelete(DeleteBehavior.Restrict);
            
        }

        public override int SaveChanges()
        {
            ApplyTimestamps();
            return base.SaveChanges();
        }

      
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }


        private void ApplyTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow; 

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedOn = now;
                }
                ((BaseEntity)entity.Entity).UpdatedOn = now;
            }
        }


    }
}
