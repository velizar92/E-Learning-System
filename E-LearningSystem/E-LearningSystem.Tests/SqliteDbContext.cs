using E_LearningSystem.Data.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace E_LearningSystem.Tests
{
    public abstract class SqliteDbContext
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";

        private readonly SqliteConnection _connection;

        protected readonly ELearningSystemDbContext DbContext;

        protected SqliteDbContext()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<ELearningSystemDbContext>()
                     .UseSqlite(_connection)
                     .Options;
            DbContext = new ELearningSystemDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
