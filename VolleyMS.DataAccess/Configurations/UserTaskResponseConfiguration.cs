using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class UserTaskResponseConfiguration : IEntityTypeConfiguration<UserTaskResponse>
    {
        public void Configure(EntityTypeBuilder<UserTaskResponse> builder)
        {
            builder.ToTable("UserTaskResponses");

            builder.HasKey(utr => utr.Id);
            builder.Property(utr => utr.Id).ValueGeneratedNever();

            builder.Property(utr => utr.Text).HasMaxLength(2000);

            // usertask
            builder.HasOne(utr => utr.UserTask)
                .WithMany(ut => ut.TaskResponses)
                .HasForeignKey(utr => utr.UserTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // attachments
            builder.HasMany(utr => utr.Attachments)
                .WithOne(a => a.UserTaskResponse)
                .HasForeignKey(a => a.UserTaskResponseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(UserTaskResponse.Attachments))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
