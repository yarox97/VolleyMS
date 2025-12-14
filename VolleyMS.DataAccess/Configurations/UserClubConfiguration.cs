using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    internal class UserClubConfiguration : IEntityTypeConfiguration<UserClub>
    {
        public void Configure(EntityTypeBuilder<UserClub> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserClubs)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.Club)
                .WithMany(c => c.UserClubs)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
