using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class JoinClubConfiguration : IEntityTypeConfiguration<JoinClub>
    {
        public void Configure(EntityTypeBuilder<JoinClub> builder)
        {
            builder.HasKey(j => j.Id);

            builder.HasOne(j => j.User)
                   .WithMany(u => u.JoinClubRequests)
                   .HasForeignKey(j => j.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(j => j.Club)
                    .WithMany(c => c.JoinClubRequests)
                    .HasForeignKey(j => j.ClubId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
