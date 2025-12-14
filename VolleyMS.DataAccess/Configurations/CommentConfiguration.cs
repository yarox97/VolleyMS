using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(c => c.Sender)
                .WithMany(u => u.SentComments)
                .HasForeignKey(c => c.SenderId);

            builder.Property(c => c.Text)
                .HasMaxLength(250);
        }
    }
}