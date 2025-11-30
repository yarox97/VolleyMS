using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Mappers
{
    public static class NotificationMapper 
    {
        public static Notification? ToDomain(NotificationEntity notificationEntity)
        {
            if (notificationEntity == null)
            {
                return null!;
            }
            var notification = Notification.Create(
                notificationEntity.Id,
                notificationEntity.RequiredClubMemberRoles,
                notificationEntity.Category,
                notificationEntity.Text,
                notificationEntity.LinkedURL,
                notificationEntity.Payload
            );
            notification.CreatorId = notificationEntity.CreatorId;
            notification.CreatedAt = notificationEntity.CreatedAt;
            notification.UpdatedAt = notificationEntity.UpdatedAt;
            notification.DeletedAt = notificationEntity.DeletedAt;

            return notification;
        }

        public static NotificationEntity? ToEntity(Notification notification, Guid? senderId)
        {
            if (notification == null)
            {
                return null;
            }
            return new NotificationEntity
            {
                Id = notification.Id,
                Text = notification.Text,
                LinkedURL = notification.LinkedURL,
                Payload = notification.Payload,
                SenderId = senderId,
                Category = notification.Category,
                RequiredClubMemberRoles = notification.RequiredClubMemberRoles,
                CreatedAt = notification.CreatedAt,
                CreatorId = notification.CreatorId,
                UpdatedAt = notification.UpdatedAt,
                DeletedAt = notification.DeletedAt,
            };
        }
    }
}
