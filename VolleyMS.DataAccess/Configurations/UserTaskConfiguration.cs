using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

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

            // user
            builder.HasOne(ut => ut.User)
                .WithMany(u => u.ReceivedTasks)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Task 
            builder.HasOne(ut => ut.Task)
                .WithMany(ut => ut.Receivers)
                .HasForeignKey(ut => ut.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // comments
            builder.HasMany(ut => ut.Comments)
                .WithOne(c => c.UserTask)
                .HasForeignKey(c => c.UserTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // responses
            builder.HasMany(ut => ut.TaskResponses)
                .WithOne()
                .HasForeignKey(utr => utr.UserTaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}