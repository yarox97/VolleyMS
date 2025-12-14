using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.AvatarUrl);
            builder.Property(c => c.BackGroundURL);
            builder.Property(c => c.Name);
            builder.Property(c => c.Description);

        }
    }
}
