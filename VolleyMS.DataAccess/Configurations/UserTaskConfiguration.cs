using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;
using Task = VolleyMS.Core.Models.Task;

namespace VolleyMS.DataAccess.Configurations
{
    public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
    {
        public void Configure(EntityTypeBuilder<UserTask> builder)
        {
            builder.ToTable("UserTasks");

            builder.HasKey(ut => ut.Id);
            builder.Property(ut => ut.Id).ValueGeneratedNever();

            builder.Property(ut => ut.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(ut => ut.CompletedAt);

            builder.HasOne<User>()
                .WithMany(u => u.ReceivedTasks)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Task>()
                .WithMany(t => t.Receivers)
                .HasForeignKey(ut => ut.TaskId);

            // proofgs
            builder.HasMany(ut => ut.Proofs)
                .WithOne()
                .HasForeignKey(p => p.UserTaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}