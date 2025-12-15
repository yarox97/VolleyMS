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

            // Обычно у join-таблицы составной ключ, если нет своего Id
            builder.HasKey(un => un.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            // Уникальность: один пользователь не получает одно уведомление дважды
            builder.HasIndex(un => new { un.UserId, un.NotificationId }).IsUnique();

            builder.Property(un => un.IsChecked).IsRequired();

            // Связи
            builder.HasOne(un => un.User)
                .WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Удалили юзера -> удалили его уведомления

            builder.HasOne(un => un.Notification)
                .WithMany(n => n.UserNotifications)
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Cascade); // Удалили уведомление -> удалили у всех
        }
    }
}