using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class TaskAttachmentConfiguration : IEntityTypeConfiguration<TaskAttachment>
    {
        public void Configure(EntityTypeBuilder<TaskAttachment> builder)
        {
            builder.ToTable("TaskAttachments");

            builder.HasKey(ta => ta.Id);

            builder.Property(ta => ta.Id)
                   .ValueGeneratedNever();

            builder.HasOne(ta => ta.Task)
                   .WithMany(t => t.Attachments) 
                   .HasForeignKey(ta => ta.TaskId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ta => ta.MediaFile)
                   .WithMany() 
                   .HasForeignKey("MediaFileId") 
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ta => ta.TaskId);
        }
    }
}
