using Microsoft.EntityFrameworkCore;
using VolleyMS.DataAccess.Configurations;
using VolleyMS.Core.Models;
using VolleyMS.Core.Common;
using Task = VolleyMS.Core.Models.Task;

namespace VolleyMS.DataAccess
{
    public class VolleyMsDbContext : DbContext
    {
        public VolleyMsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClub> UserClubs { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<JoinClubRequest> JoinClubRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClubConfiguration).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableFields>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.CreatedAt == default)
                    {
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}