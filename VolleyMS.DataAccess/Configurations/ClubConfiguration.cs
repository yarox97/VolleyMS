using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Configurations
{
    public class ClubConfiguration : IEntityTypeConfiguration<ClubEntity>
    {
        public void Configure(EntityTypeBuilder<ClubEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
