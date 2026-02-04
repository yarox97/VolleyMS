using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;
using VolleyMS.Core.Services;

namespace VolleyMS.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(u => u.UserName)
                .HasMaxLength(ValidationConfig.MAX_USERNAME_LENGTH)
                .IsRequired();
            builder.HasIndex(u => u.UserName).IsUnique();

            builder.Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Name).HasMaxLength(ValidationConfig.MAX_NAME_LENGTH).IsRequired();
            builder.Property(u => u.Surname).HasMaxLength(ValidationConfig.MAX_SURNAME_LENGTH).IsRequired();
            builder.Property(u => u.AvatarUrl).HasMaxLength(500).IsRequired(false);

            builder.Property(u => u.UserType)
                .HasConversion<string>()
                .IsRequired();

            builder.HasMany(u => u.UserClubs)
                .WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // contracts
            builder.HasMany(u => u.Contracts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // join club requests
            builder.HasMany(u => u.JoinClubRequests)
                .WithOne(j => j.User)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // notifications received
            builder.HasMany(u => u.ReceivedNotifications)
                .WithOne(un => un.User)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // received tasks 
            builder.HasMany(u => u.ReceivedTasks)
                .WithOne() 
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // sent tasks
            builder.HasMany(u => u.SentTasks)
                .WithOne(t => t.Sender)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // sent comments
            builder.HasMany(u => u.SentComments)
                .WithOne(c => c.Sender)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // sent notifications
            builder.HasMany(u => u.SentNotifications)
                .WithOne(n => n.Sender)
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}