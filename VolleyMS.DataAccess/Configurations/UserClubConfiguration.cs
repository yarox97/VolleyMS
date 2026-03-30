using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    internal class UserClubConfiguration : IEntityTypeConfiguration<UserClub>
    {
        public void Configure(EntityTypeBuilder<UserClub> builder)
        {
            builder.ToTable("UserClubs");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            // combined fc 
            builder.HasIndex(uc => new { uc.UserId, uc.ClubId })
                .IsUnique();

            builder.Property(uc => uc.ClubMemberRole)
                .HasConversion<string>() 
                .HasMaxLength(50)
                .IsRequired();

            // user relation 
            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserClubs)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // club relation
            builder.HasOne(uc => uc.Club)
                .WithMany(c => c.UserClubs)
                .HasForeignKey(uc => uc.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
