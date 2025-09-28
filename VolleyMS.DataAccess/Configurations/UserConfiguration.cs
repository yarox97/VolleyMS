using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(u => u.UserName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Password)
                .IsRequired();

            builder.Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Surname)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(u => u.ContractModels)
                .WithOne(c => c.UserModel)
                .HasForeignKey(c => c.UserId);

            builder.HasMany(u => u.ClubModels)
                .WithMany(c => c.UserModels);

            builder.HasMany(u => u.ReceiverTaskModels)
                .WithMany(t => t.UserModel_receivers);

            builder.HasMany(u => u.SenderTaskModels)
                .WithOne(t => t.UserModel_sender)
                .HasForeignKey(t => t.SenderId);

            builder.HasMany(u => u.CommentModels)
                .WithOne(c => c.UserModel_sender)
                .HasForeignKey(c => c.SenderId);
        }
    }
}