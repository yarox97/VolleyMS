using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VolleyMS.Core.Models;
using Task = VolleyMS.Core.Models.Task;

namespace VolleyMS.DataAccess.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(x => x.Id);

            builder.Property(t => t.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            // --- 1. Конвертация Enum-ов ---
            builder.Property(t => t.TaskType)
                .HasConversion<string>()
                .IsRequired();

            // Внимание: TaskStatus системный enum? Используйте свой из Core.Shared
            builder.Property(t => t.TaskStatus)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(t => t.PenaltyType)
                .HasConversion<string>()
                .IsRequired();

            // --- 2. Конвертация списка дней недели (List<DayOfWeek>) ---
            // Храним как строку: "Monday,Friday"

            var daysConverter = new ValueConverter<List<DayOfWeek>, string>(
                v => string.Join(",", v),
                v => string.IsNullOrEmpty(v)
                    ? new List<DayOfWeek>()
                    : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(d => Enum.Parse<DayOfWeek>(d))
                       .ToList()
            );

            var daysComparer = new ValueComparer<List<DayOfWeek>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            builder.Property(t => t.DayOfWeek)
                .HasConversion(daysConverter)
                .Metadata.SetValueComparer(daysComparer);


            // --- 3. Связи ---

            // Клуб (Обязательная связь)
            builder.HasOne(t => t.Club)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // Отправитель (Автор задачи)
            builder.HasOne(t => t.Sender)
                .WithMany(u => u.SentTasks)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Не удаляем задачу, если удален автор (история важна)

            // Получатели (Many-to-Many)
            // Создаем таблицу связей TaskReceivers
            builder.HasMany(t => t.Receivers)
                .WithMany(u => u.ReceivedTasks)
                .UsingEntity(j => j.ToTable("TaskReceivers"));

            // Комментарии
            builder.HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}