using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            // Текст комментария
            builder.Property(c => c.Text)
                .HasMaxLength(500) // 250 может быть маловато, поставил 500
                .IsRequired();

            // --- Связи ---

            // 1. Связь с Задачей
            // Если задачу удалили, комментарии к ней не имеют смысла -> удаляем (Cascade)
            builder.HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Связь с Отправителем
            // Если удаляют юзера, мы НЕ хотим, чтобы пропали комментарии в задачах.
            // Поэтому Restrict: БД не даст удалить юзера, пока у него есть комментарии.
            // (Либо можно сделать SetNull, если SenderId будет nullable)
            builder.HasOne(c => c.Sender)
                .WithMany(u => u.SentComments)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}