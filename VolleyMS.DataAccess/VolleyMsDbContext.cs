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

        public DbSet<ClubModel> Clubs { get; set; } 
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ContractModel> Contracts { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClubConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
