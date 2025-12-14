using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;
using Task = VolleyMS.Core.Models.Task;

namespace VolleyMS.DataAccess.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(t => t.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(50) 
                .IsRequired();

            builder.HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId);
            
            builder.HasOne(t => t.Sender)
                .WithMany(u => u.SentTasks)
                .HasForeignKey(t => t.SenderId);

            builder.HasMany(t => t.Receivers)
                .WithMany(u => u.ReceivedTasks);

            builder.HasOne(t => t.Club)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ClubId);
        }
    }
}