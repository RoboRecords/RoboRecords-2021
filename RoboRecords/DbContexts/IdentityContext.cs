using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoboRecords.Models;

namespace RoboRecords.DatabaseContexts
{
    public class IdentityContext : IdentityDbContext<IdentityRoboUser>
    {
        private static string _connectionString;

        public IdentityContext()
        {
        }
        
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Required limits on columns to avoid "key too long" error when creating database.
            // We'll never hit these anyway, the default limit is 767 characters, ridiculous. --- Zenya
            builder.Entity<IdentityRoboUser>(entity => entity.Property(m => m.Id).HasMaxLength(256));
            builder.Entity<IdentityRoboUser>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(256));
            builder.Entity<IdentityRoboUser>(entity => entity.Property(m => m.Email).HasMaxLength(256));
            builder.Entity<IdentityRoboUser>(entity => entity.Property(m => m.NormalizedUserName).HasMaxLength(256));
            builder.Entity<IdentityRoboUser>(entity => entity.Property(m => m.ApiKey));
            builder.Entity<IdentityRoboUser>(entity => entity.Property(m => m.Roles));

            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(256));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(256));

            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(256));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(256));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(256));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(256));

            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(256));

            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(256));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(256));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(256));

            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(256));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(256));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(256));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(256));
        }
    }
}