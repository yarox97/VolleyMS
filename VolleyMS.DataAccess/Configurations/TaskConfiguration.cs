using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VolleyMS.Core.Models;
using VolleyMS.Core.Services;
using Task = VolleyMS.Core.Models.Task;

namespace VolleyMS.DataAccess.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(t => t.Title)
                .HasMaxLength(ValidationConfig.MAX_TASK_TITLE)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(10000)
                .IsRequired(false);

            // Enum-ы самой задачи
            builder.Property(t => t.TaskType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(t => t.PenaltyType)
                .HasConversion<string>()
                .IsRequired();

            // --- MAPPING VALUE OBJECT (TaskSchedule) ---
            builder.OwnsOne(t => t.TaskSchedule, sb =>
            {
                sb.Property(s => s.StartDate).HasColumnName("StartDate").IsRequired();
                sb.Property(s => s.EndDate).HasColumnName("EndDate");
                sb.Property(s => s.StartTime).HasColumnName("StartTime").IsRequired();
                sb.Property(s => s.EndTime).HasColumnName("EndTime").IsRequired();

                sb.Property(s => s.Days)
                  .HasColumnName("DaysOfWeek")
                  .IsRequired();
            });

            // club
            builder.HasOne(t => t.Club)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // sender
            builder.HasOne(t => t.Sender)
                .WithMany(u => u.SentTasks)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // receivers
            builder.HasMany(t => t.Receivers)
                .WithOne(ut => ut.Task)
                .HasForeignKey(ut => ut.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}