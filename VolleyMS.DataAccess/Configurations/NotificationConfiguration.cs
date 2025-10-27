using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.DataAccess.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(n => n.Text)
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasOne(n => n.Sender)
                .WithMany(u => u.SentNotifications)
                .HasForeignKey(n => n.senderId);

            builder.HasMany(n => n.Receivers)
           .WithMany(u => u.ReceivedNotifications);
        }
    }
}
