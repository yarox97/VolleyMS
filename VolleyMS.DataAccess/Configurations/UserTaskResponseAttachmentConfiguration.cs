using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class UserTaskResponseAttachmentConfiguration : IEntityTypeConfiguration<UserTaskResponseAttachment>
    {
        public void Configure(EntityTypeBuilder<UserTaskResponseAttachment> builder)
        {
            builder.ToTable("UserTaskResponseAttachments");

            builder.Property<Guid>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");

            // response 
            builder.HasOne(uta => uta.UserTaskResponse)
                .WithMany(utr => utr.Attachments)
                .HasForeignKey(uta => uta.UserTaskResponseId);

            // mediafile 
            builder.HasOne(uta => uta.Media)
                .WithMany()
                .HasForeignKey(uta => uta.MediaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
