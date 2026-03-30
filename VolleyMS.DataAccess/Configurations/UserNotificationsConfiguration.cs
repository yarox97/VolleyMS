using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    internal sealed class UserNotificationsConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("UserNotifications");

            builder.HasKey(un => un.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            // ensure userNotification uniqueness 
            builder.HasIndex(un => new { un.UserId, un.NotificationId }).IsUnique();

            builder.Property(un => un.IsChecked).IsRequired();

            // user rel
            builder.HasOne(un => un.User)
                .WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // notification rel
            builder.HasOne(un => un.Notification)
                .WithMany(n => n.UserNotifications)
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}