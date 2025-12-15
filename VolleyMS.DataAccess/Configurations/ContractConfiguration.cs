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

            builder.Property(c => c.MonthlySalary)
                .HasPrecision(18, 2)
                .IsRequired(false);

            builder.Property(c => c.Currency)
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(c => c.BeginsFrom).IsRequired();
            builder.Property(c => c.EndsBy).IsRequired();

            // 3. Связь с Пользователем
            builder.HasOne(c => c.User)
                .WithMany(u => u.Contracts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Удалили юзера -> удалили контракты

            // 4. Связь с Клубом
            builder.HasOne(c => c.Club)
                .WithMany() 
                .HasForeignKey(c => c.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}