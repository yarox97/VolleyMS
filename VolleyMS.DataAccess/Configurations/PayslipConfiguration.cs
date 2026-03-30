using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models.Payslips;

namespace VolleyMS.DataAccess.Configurations
{
    public class PayslipConfiguration : IEntityTypeConfiguration<Payslip>
    {
        public void Configure(EntityTypeBuilder<Payslip> builder)
        {
            builder.ToTable("Payslips");

            builder.HasKey(p => p.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(p => p.BaseSalary)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Ignore(p => p.TotalAmount);

            builder.HasIndex(p => new { p.ContractId, p.Month, p.Year }).IsUnique();

            builder.HasIndex(p => new { p.UserId, p.Month, p.Year });

            builder.Navigation(p => p.Adjustments)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
