using RestApi.Models;
using SQLite.CodeFirst;
using System.Data.Entity;

namespace RestApi.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext() : base("name=AuthDbConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // ✅ SQLite CodeFirst 초기화기 등록
            var sqliteInitializer = new SqliteCreateDatabaseIfNotExists<AuthDbContext>(modelBuilder);
            Database.SetInitializer(sqliteInitializer);

            // ✅ 인덱스 정의 (Fluent API)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(r => r.UserId);

            modelBuilder.Entity<AccessToken>()
                .HasIndex(a => new { a.UserId, a.Token });

            modelBuilder.Entity<AccessToken>()
                .HasIndex(a => a.UserId);

            base.OnModelCreating(modelBuilder);
        }

        public class AuthDbContextInitializer : SqliteCreateDatabaseIfNotExists<AuthDbContext>
        {
            public AuthDbContextInitializer(DbModelBuilder modelBuilder)
                : base(modelBuilder)
            {
            }
        }

    }
}
