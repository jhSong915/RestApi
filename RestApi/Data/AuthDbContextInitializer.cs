using SQLite.CodeFirst;
using System.Data.Entity;

namespace RestApi.Data
{
    public class AuthDbContextInitializer : SqliteCreateDatabaseIfNotExists<AuthDbContext>
    {
        public AuthDbContextInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }
    }
}
