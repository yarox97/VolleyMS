using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class JoinClubConfiguration : IEntityTypeConfiguration<JoinClubRequest>
    {
        public void Configure(EntityTypeBuilder<JoinClubRequest> builder)
        {
            // 1. Имя таблицы
            builder.ToTable("JoinClubRequests");

            // 2. Первичный ключ
            builder.HasKey(j => j.Id);

            // 3. Настройка Enum (храним как строку для читаемости: "Pending", "Approved")
            builder.Property(j => j.JoinClubRequestStatus)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            // 4. Связь с Заявителем (User)
            // Если удаляют пользователя -> удаляется и его заявка (Cascade)
            builder.HasOne(j => j.User)
                .WithMany(u => u.JoinClubRequests) // У User должна быть коллекция JoinClubRequests
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 5. Связь с Клубом (Club)
            // Если удаляют клуб -> удаляется заявка (Cascade)
            builder.HasOne(j => j.Club)
                .WithMany(c => c.JoinClubRequests)
                .HasForeignKey(j => j.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // 6. Связь с Ответственным (Responser) - ВАЖНО
            // Это Nullable связь. 
            // Мы используем .WithMany(), но без параметров, так как у User 
            // скорее всего нет списка "ЗаявкиКоторыеЯОдобрил".
            builder.HasOne(j => j.Responser)
                .WithMany()
                .HasForeignKey(j => j.ResponserId)
                .IsRequired(false) // Поле может быть null
                .OnDelete(DeleteBehavior.ClientSetNull);
            // Важно: ClientSetNull или Restrict. 
            // Не используйте Cascade! Если админ удалится, заявка должна остаться, 
            // просто поле ResponserId станет null.
        }
    }
}
