using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Common;
using VolleyMS.Core.Models;
using VolleyMS.Core.Models.Payslips;
using VolleyMS.DataAccess.Configurations;
using VolleyMS.DataAccess.Outbox;
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
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<UserTaskResponse> UserTaskResponses { get; set; }
        public DbSet<UserTaskResponseAttachment> UserTaskResponseAttachments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<JoinClubRequest> JoinClubRequests { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public DbSet<PayslipAdjustment> PayslipAdjustments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ClubConfiguration).Assembly);
            // stosujemy konfiguracje z całego assembly w którym znajduje się ClubConfiguration

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OutboxMessage>(b =>
            {
                b.ToTable("OutboxMessages");
                b.HasKey(e => e.Id);
            });

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