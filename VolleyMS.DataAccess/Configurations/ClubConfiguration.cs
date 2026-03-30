using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;
using VolleyMS.Core.Services;

namespace VolleyMS.DataAccess.Configurations
{
    public class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.ToTable("Clubs");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(ValidationConfig.MAX_CLUB_NAME_LENGTH);

            builder.Property(c => c.JoinCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasIndex(c => c.JoinCode)
                .IsUnique();

            builder.Property(c => c.Description)
                .HasMaxLength(5000)
                .IsRequired(false);

            builder.Property(c => c.AvatarUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(c => c.BackGroundURL)
                .HasMaxLength(500)
                .IsRequired(false);

            // userclub relation
            builder.HasMany(c => c.UserClubs)
                .WithOne(uc => uc.Club)
                .HasForeignKey(uc => uc.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // join club request rel
            builder.HasMany(c => c.JoinClubRequests)
                .WithOne(r => r.Club)
                .HasForeignKey(r => r.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // task rel
            builder.HasMany(c => c.Tasks)
                .WithOne() 
                .HasForeignKey("ClubId") 
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
