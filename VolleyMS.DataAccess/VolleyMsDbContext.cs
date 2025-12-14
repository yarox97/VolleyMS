using Microsoft.EntityFrameworkCore;
using VolleyMS.DataAccess.Configurations;
using VolleyMS.Core.Models;
using Task = VolleyMS.Core.Models.Task;

namespace VolleyMS.DataAccess
{
    public class VolleyMsDbContext : DbContext
    {
        public VolleyMsDbContext(DbContextOptions options) 
            : base(options)
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
        public DbSet<JoinClub> JoinClubRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClubConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new UserClubConfiguration());
            modelBuilder.ApplyConfiguration(new UserNotificationsConfiguration());
            modelBuilder.ApplyConfiguration(new JoinClubConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
