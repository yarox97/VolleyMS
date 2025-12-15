using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    internal class UserClubConfiguration : IEntityTypeConfiguration<UserClub>
    {
        public void Configure(EntityTypeBuilder<UserClub> builder)
        {
            builder.ToTable("UserClubs");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            // 1. ГЛАВНОЕ: Уникальный составной индекс
            // Гарантирует, что пользователь не вступит в один клуб дважды.
            builder.HasIndex(uc => new { uc.UserId, uc.ClubId })
                .IsUnique();

            // 2. Конвертация Enum
            builder.Property(uc => uc.ClubMemberRole)
                .HasConversion<string>() // Храним как "Player", "Coach"
                .HasMaxLength(50)
                .IsRequired();

            // 3. Связи
            // Удаление пользователя -> удаляет запись о членстве
            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserClubs)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Удаление клуба -> удаляет всех участников из таблицы связей
            builder.HasOne(uc => uc.Club)
                .WithMany(c => c.UserClubs)
                .HasForeignKey(uc => uc.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
