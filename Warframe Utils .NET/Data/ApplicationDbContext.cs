using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Warframe_Utils_.NET.Data
{
    /// <summary>
    /// ApplicationDbContext - Entity Framework Core database context for the application.
    /// 
    /// Inherits from IdentityDbContext which provides:
    /// - User management tables (AspNetUsers)
    /// - Role management tables (AspNetRoles)
    /// - User-to-role mappings (AspNetUserRoles)
    /// - Claims, tokens, and logins tables
    /// 
    /// This application doesn't currently use additional custom tables beyond Identity,
    /// but the structure is in place to add them if needed in the future.
    /// 
    /// Database: SQL Server LocalDB (development) or production SQL Server
    /// Connection String: Configured in appsettings.json
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Constructor for ApplicationDbContext.
        /// 
        /// The framework automatically provides DbContextOptions via dependency injection.
        /// Options include:
        /// - Database provider (SQL Server)
        /// - Connection string
        /// - Other configuration (migrations, logging, etc.)
        /// </summary>
        /// <param name="options">Database context options configured in Program.cs</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ============================================================================
        // DATABASE TABLES (DbSets)
        // ============================================================================
        // Currently, only Identity tables are used (inherited from IdentityDbContext)
        
        // To add custom tables in the future, add DbSet properties here:
        // Example:
        // public DbSet<UserFavorite>? UserFavorites { get; set; }
        // public DbSet<PriceHistory>? PriceHistories { get; set; }

        // ============================================================================
        // MODEL CONFIGURATION
        // ============================================================================
        // Override OnModelCreating to configure table mappings, constraints, etc.
        // Example usage:
        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure custom tables
            builder.Entity<UserFavorite>()
                .HasKey(uf => new { uf.UserId, uf.ModUrl });

            // Add indexes for better query performance
            builder.Entity<PriceHistory>()
                .HasIndex(ph => ph.ModUrl)
                .IsUnique(false);
        }
        */
    }
}
