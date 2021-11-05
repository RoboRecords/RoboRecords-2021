using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoboRecords.Models;

namespace RoboRecords.DatabaseContexts
{
    public class IdentityContext : IdentityDbContext<IdentityUser>
    {
        private static string _connectionString;

        public DbSet<RoboUser> RoboSignedUsers { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}