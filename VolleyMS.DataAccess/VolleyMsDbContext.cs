using Microsoft.EntityFrameworkCore;
using VolleyMS.DataAccess.Configurations;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess
{
    public class VolleyMsDbContext : DbContext
    {
        public VolleyMsDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<ClubEntity> Clubs { get; set; } 
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserClubsEntity> UserClubs { get; set; }
        public DbSet<ContractEntity> Contracts { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<UserNotificationsEntity> UserNotifications { get; set; }
        public DbSet<JoinClubEntity> JoinClubRequests { get; set; }

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
