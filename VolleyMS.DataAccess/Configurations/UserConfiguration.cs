using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            // 1. Скалярные свойства
            builder.Property(u => u.UserName)
                .HasMaxLength(50)
                .IsRequired();

            // UserName должен быть уникальным
            builder.HasIndex(u => u.UserName).IsUnique();

            builder.Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();

            // Email должен быть уникальным
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Name).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Surname).HasMaxLength(50).IsRequired();
            builder.Property(u => u.AvatarUrl).HasMaxLength(500).IsRequired(false);

            // Конвертация Enum UserType в строку ("Admin", "Player")
            builder.Property(u => u.UserType)
                .HasConversion<string>()
                .IsRequired();

            // 2. Настройка коллекций и связей

            // --- Клубы (UserClubs) ---
            // Если удаляем юзера, удаляется его членство в клубах
            builder.HasMany(u => u.UserClubs)
                .WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Контракты (Contracts) ---
            // Если удаляем юзера, удаляются его контракты
            builder.HasMany(u => u.Contracts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Заявки на вступление (JoinClubRequests) ---
            // Заявки пользователя
            builder.HasMany(u => u.JoinClubRequests)
                .WithOne(j => j.User)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Входящие уведомления (ReceivedNotifications - таблица UserNotification) ---
            builder.HasMany(u => u.ReceivedNotifications)
                .WithOne(un => un.User)
                .HasForeignKey(un => un.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Входящие задачи (ReceivedTasks - Many-to-Many) ---
            // Предполагается, что у Task есть коллекция Receivers.
            // Создаем скрытую таблицу связей "TaskReceivers"
            builder.HasMany(u => u.ReceivedTasks)
                .WithMany(t => t.Receivers)
                .UsingEntity(j => j.ToTable("TaskReceivers"));

            // 3. Связь "Отправитель" (Sent...)
            // Для отправленных данных мы используем Restrict.
            // Нельзя удалить пользователя, если он создал важные задачи или комментарии,
            // пока эти данные не будут удалены или переназначены.
            // Это защищает целостность истории.

            builder.HasMany(u => u.SentTasks)
                .WithOne(t => t.Sender)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.SentComments)
                .WithOne(c => c.Sender)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.SentNotifications)
                .WithOne(n => n.Sender)
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}