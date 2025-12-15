using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(n => n.Text)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(n => n.LinkedURL)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(n => n.Payload)
                .HasMaxLength(2000) // JSON payload может быть длинным
                .IsRequired(false);

            // --- 1. Конвертация Категории (Enum) ---
            builder.Property(n => n.Category)
                .HasConversion<string>()
                .IsRequired();

            // --- 2. СЛОЖНЫЙ МОМЕНТ: Список Ролей (List<Enum>) ---
            // База данных не хранит массивы. Мы сохраним это как строку, разделенную запятыми.
            // Пример в базе: "Player,Coach,Admin"

            var rolesConverter = new ValueConverter<IList<ClubMemberRole>, string>(
                // В базу: List -> String
                v => string.Join(",", v),
                // Из базы: String -> List (с проверкой на пустоту)
                v => string.IsNullOrEmpty(v)
                    ? new List<ClubMemberRole>()
                    : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(e => Enum.Parse<ClubMemberRole>(e))
                       .ToList()
            );

            // Применяем компаратор, чтобы EF Core понимал, изменился ли список
            var rolesComparer = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<IList<ClubMemberRole>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            builder.Property(n => n.RequiredClubMemberRoles)
                .HasConversion(rolesConverter)
                .Metadata.SetValueComparer(rolesComparer);

            // --- 3. Связь с Отправителем (Sender) ---
            // Sender может быть null (системное уведомление)
            builder.HasOne(n => n.Sender)
                .WithMany(u => u.SentNotifications)
                .HasForeignKey(n => n.SenderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); // Не удаляем уведомление, если удален отправитель

            // --- 4. Связь с Получателями (UserNotifications) ---
            builder.HasMany(n => n.UserNotifications)
                .WithOne(un => un.Notification)
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Cascade); // Если удалили само уведомление, удаляем записи о доставке

            // Доступ к полю
            builder.Metadata.FindNavigation(nameof(Notification.UserNotifications))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
