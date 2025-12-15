using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.ToTable("Clubs");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            // Настройка свойств
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100); // Ограничиваем длину имени

            builder.Property(c => c.JoinCode)
                .IsRequired()
                .HasMaxLength(10); // Код обычно короткий

            // Важно: Код для вступления должен быть уникальным в системе
            builder.HasIndex(c => c.JoinCode)
                .IsUnique();

            builder.Property(c => c.Description)
                .HasMaxLength(500)
                .IsRequired(false); // Может быть null

            builder.Property(c => c.AvatarUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(c => c.BackGroundURL)
                .HasMaxLength(500)
                .IsRequired(false);

            // Настройка связей (One-to-Many)

            // Связь с UserClub
            // Мы говорим EF: "У Клуба много UserClubs, у UserClub один Клуб".
            // При удалении Клуба -> удаляются все записи о членстве (Cascade).
            builder.HasMany(c => c.UserClubs)
                .WithOne(uc => uc.Club)
                .HasForeignKey(uc => uc.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с JoinClubRequests
            builder.HasMany(c => c.JoinClubRequests)
                .WithOne(r => r.Club)
                .HasForeignKey(r => r.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с Tasks (если сущность Task определена в вашей модели)
            builder.HasMany(c => c.Tasks)
                .WithOne() 
                .HasForeignKey("ClubId") // Или явно укажите свойство, если оно есть в Task
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
