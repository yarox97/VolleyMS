using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class TaskProofConfiguration : IEntityTypeConfiguration<TaskProof>
    {
        public void Configure(EntityTypeBuilder<TaskProof> builder)
        {
            builder.ToTable("TaskProofs");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.Property(p => p.Url).IsRequired().HasMaxLength(2048);
            builder.Property(p => p.Name).HasMaxLength(255);

            builder.Property(p => p.Type)
                .HasConversion<string>() // Или <int>, если хотите хранить цифры
                .IsRequired();
        }
    }
}