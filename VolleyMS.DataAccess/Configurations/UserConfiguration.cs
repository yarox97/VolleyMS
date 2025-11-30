using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(u => u.UserName).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Name).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Surname).HasMaxLength(50).IsRequired();

            builder.HasMany(u => u.Contracts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(u => u.ReceivedTasks)
                .WithMany(t => t.Receivers);

            builder.HasMany(u => u.SentTasks)
                .WithOne(t => t.Sender)
                .HasForeignKey(t => t.SenderId);

            builder.HasMany(u => u.SentComments)
                .WithOne(c => c.Sender)
                .HasForeignKey(c => c.SenderId);

            builder.HasMany(u => u.ReceivedNotifications)
                .WithOne(un => un.User)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.SentNotifications)
                .WithOne(n => n.Sender)
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}