using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Models.Payslips;

namespace VolleyMS.DataAccess.Configurations
{
    public class PayslipAdjustmentConfiguration : IEntityTypeConfiguration<PayslipAdjustment>
    {
        public void Configure(EntityTypeBuilder<PayslipAdjustment> builder)
        {
            builder.ToTable("PayslipAdjustments");

            builder.HasKey(a => a.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(a => a.Amount)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            builder.Property(a => a.Reason)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(a => a.SourceTaskId)
                .IsRequired(false);

            builder.HasOne(a => a.Payslip)
                .WithMany(p => p.Adjustments)
                .HasForeignKey(a => a.PayslipId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
