using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<ContractModel>
    {
        public void Configure(EntityTypeBuilder<ContractModel> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(c => c.UserModel)
                .WithMany(u => u.ContractModels)
                .HasForeignKey(c => c.UserId);
        }
    }
}