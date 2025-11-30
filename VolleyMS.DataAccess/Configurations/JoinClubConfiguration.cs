using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Configurations
{
    public class JoinClubConfiguration : IEntityTypeConfiguration<JoinClubEntity>
    {
        public void Configure(EntityTypeBuilder<JoinClubEntity> builder)
        {
            builder.HasKey(j => j.Id);

            builder.HasOne(j => j.UserEntity)
                   .WithMany(u => u.JoinClubRequests)
                   .HasForeignKey(j => j.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(j => j.ClubEntity)
                    .WithMany(c => c.JoinClubRequests)
                    .HasForeignKey(j => j.ClubId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
