using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;
using VolleyMS.Core.Services;

namespace VolleyMS.DataAccess.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(c => c.Text)
                .HasMaxLength(ValidationConfig.MAX_COMMENT_TEXT)
                .IsRequired();

            builder.HasOne<UserTask>()
                .WithMany(ut => ut.Comments) 
                .HasForeignKey("UserTaskId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Sender)
                .WithMany(u => u.SentComments)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}