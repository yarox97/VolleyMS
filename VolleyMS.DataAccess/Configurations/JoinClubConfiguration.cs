using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class JoinClubConfiguration : IEntityTypeConfiguration<JoinClubRequest>
    {
        public void Configure(EntityTypeBuilder<JoinClubRequest> builder)
        {
            builder.ToTable("JoinClubRequests");

            builder.HasKey(j => j.Id);

            builder.Property(j => j.Id)
                .ValueGeneratedNever();

            builder.Property(j => j.JoinClubRequestStatus)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            // user relationship
            builder.HasOne(j => j.User)
                .WithMany(u => u.JoinClubRequests)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // club relationship
            builder.HasOne(j => j.Club)
                .WithMany(c => c.JoinClubRequests)
                .HasForeignKey(j => j.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // responser rel 
            builder.HasOne(j => j.Responser)
                .WithMany()
                .HasForeignKey(j => j.ResponserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
