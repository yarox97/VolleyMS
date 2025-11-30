using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Configurations
{
    internal sealed class UserNotificationsConfiguration : IEntityTypeConfiguration<UserNotificationsEntity>
    {
        public void Configure(EntityTypeBuilder<UserNotificationsEntity> builder)
        {
            builder.HasKey(un => new { un.UserId, un.NotificationId });

            builder.HasOne(un => un.Notification)
                .WithMany(n => n.UserNotifications)
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(un => un.User)
                .WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}