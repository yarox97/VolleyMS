using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<ContractEntity>
    {
        public void Configure(EntityTypeBuilder<ContractEntity> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(c => c.User)
                .WithMany(u => u.Contracts)
                .HasForeignKey(c => c.UserId);
        }
    }
}