using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<CommentModel>
    {
        public void Configure(EntityTypeBuilder<CommentModel> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(c => c.UserModel_sender)
                .WithMany(u => u.CommentModels)
                .HasForeignKey(c => c.UserModel_sender.Id);

            builder.Property(c => c.Text)
                .HasMaxLength(250);
        }
    }
}