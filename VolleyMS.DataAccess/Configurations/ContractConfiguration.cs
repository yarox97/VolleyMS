using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contracts");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            // salary properties
            builder.Property(c => c.MonthlySalary)
                .HasPrecision(18, 2)
                .IsRequired(false);
            // currency propertiws
            builder.Property(c => c.Currency)
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();
            // schedule properties
            builder.Property(c => c.BeginsFrom).IsRequired();
            builder.Property(c => c.EndsBy).IsRequired();
            // user relationship
            builder.HasOne(c => c.User)
                .WithMany(u => u.Contracts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // assigner relationship
            builder.HasOne(c => c.Club)
                .WithMany() 
                .HasForeignKey(c => c.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}