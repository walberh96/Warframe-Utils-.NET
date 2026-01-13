using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Warframe_Utils_.NET.Models;

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
    /// Custom tables for Price Alert System:
    /// - PriceAlerts: User price monitoring alerts
    /// - AlertNotifications: Notifications when alerts are triggered
    /// 
    /// Database: PostgreSQL
    /// Connection String: Configured in appsettings.json
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Constructor for ApplicationDbContext.
        /// 
        /// The framework automatically provides DbContextOptions via dependency injection.
        /// Options include:
        /// - Database provider (PostgreSQL)
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
        /// <summary>
        /// Price alerts created by users to monitor item prices
        /// </summary>
        public DbSet<PriceAlert> PriceAlerts { get; set; }

        /// <summary>
        /// Notifications sent to users when price alerts are triggered
        /// </summary>
        public DbSet<AlertNotification> AlertNotifications { get; set; }

        // ============================================================================
        // MODEL CONFIGURATION
        // ============================================================================
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure PriceAlert table
            builder.Entity<PriceAlert>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ItemId)
                    .HasMaxLength(500);

                entity.Property(e => e.UserId)
                    .IsRequired();

                // Index for quick lookup by user
                entity.HasIndex(e => e.UserId);

                // Index for active alerts (for background service)
                entity.HasIndex(e => new { e.IsActive, e.IsTriggered });
            });

            // Configure AlertNotification table
            builder.Entity<AlertNotification>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.Message)
                    .IsRequired();

                // Foreign key relationship
                entity.HasOne(e => e.PriceAlert)
                    .WithMany()
                    .HasForeignKey(e => e.PriceAlertId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Index for quick lookup by user
                entity.HasIndex(e => e.UserId);

                // Index for unread notifications
                entity.HasIndex(e => new { e.UserId, e.IsRead });
            });
        }
    }
}
