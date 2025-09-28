using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Entities;
using VolleyMS.DataAccess.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(t => t.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(50) 
                .IsRequired();

            builder.HasMany(t => t.CommentModels)
                .WithOne(c => c.TaskModel)
                .HasForeignKey(c => c.TaskModel.Id);
            
            builder.HasOne(t => t.UserModel_sender)
                .WithMany(s => s.TaskModels)
                .HasForeignKey(t => t.SenderId);

            builder.HasOne(t => t.ClubModel)
                .WithMany(c => c.TaskModels)
                .HasForeignKey(t => t.ClubId);
        }
    }
}