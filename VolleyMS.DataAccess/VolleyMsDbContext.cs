using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.DataAccess.Configurations;
using VolleyMS.DataAccess.Entities;
using VolleyMS.DataAccess.Models;

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
        public DbSet<User_ClubsEntity> UserClubs { get; set; }
        public DbSet<ContractEntity> Contracts { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClubConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new User_ClubConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
