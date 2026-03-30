using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(n => n.Text)
                .HasMaxLength(10000)
                .IsRequired();

            builder.Property(n => n.LinkedURL)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(n => n.Payload)
                .HasMaxLength(2000) // JSON payload 
                .IsRequired(false);

            // catergory convert 
            builder.Property(n => n.Category)
                .HasConversion<string>()
                .IsRequired();

            var rolesConverter = new ValueConverter<IList<ClubMemberRole>, string>(
                v => string.Join(",", v), // do bazy jako string 

                v => string.IsNullOrEmpty(v)
                    ? new List<ClubMemberRole>()
                    : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(e => Enum.Parse<ClubMemberRole>(e))
                       .ToList()
            );

            // from db separated string converter to list of clubMemberRoles 
            var rolesComparer = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<IList<ClubMemberRole>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            builder.Property(n => n.RequiredClubMemberRoles)
                .HasConversion(rolesConverter)
                .Metadata.SetValueComparer(rolesComparer);

            // sender relationship
            builder.HasOne(n => n.Sender)
                .WithMany(u => u.SentNotifications)
                .HasForeignKey(n => n.SenderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); 

            // user notifications mxn relation
            builder.HasMany(n => n.UserNotifications)
                .WithOne(un => un.Notification)
                .HasForeignKey(un => un.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Notification.UserNotifications))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
