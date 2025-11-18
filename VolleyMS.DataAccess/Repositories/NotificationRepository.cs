using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Entities;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;
using VolleyMS.DataAccess.Models;
using Task = System.Threading.Tasks.Task;


namespace VolleyMS.DataAccess.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly VolleyMsDbContext _context;
        public NotificationRepository(VolleyMsDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Create(Notification notification, IList<Guid> receivers, Guid senderId)
        {
            var senderEntity = await _context.Users.FindAsync(senderId) ?? throw new Exception("Sender not found!");

            var receiversEntities = await _context.Users
               .Where(u => receivers.Contains(u.Id))
               .ToListAsync();

            if (receiversEntities.Count < 1) throw new Exception("No recievers found!");

            var NotificationModel = new NotificationEntity 
            {
                notificationType = new NotificationTypeEntity 
                {
                    Id = notification.NotificationType.Id,
                    notificationCategory = notification.NotificationType.notificationCategory,
                    requiredClubMemberRole = notification.NotificationType.requiredClubMemberRole
                },
                Text = notification.Text,
                isChecked = notification.IsChecked,
                LinkedURL = notification.LinkedURL,
                senderId = senderId,
                Sender = senderEntity,
                Receivers = receiversEntities,
                CreatorId = senderId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(NotificationModel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                throw;
            }
            return notification.Id;
        }

        public async Task Delete(Guid NorificationId)
        {
            await _context.Notifications
                .Where(n => n.Id == NorificationId)
                .ExecuteDeleteAsync();
        }

        public async Task<IList<Notification>> GetUserNotifications(string userName)
        {
            var notifEntities = await _context.Notifications
                .Include(n => n.notificationType)
                .Where(n => n.Receivers.Any(r => r.UserName == userName))
                .ToListAsync();

            var notifications = new List<Notification>();
            foreach (var notifEntity in notifEntities)
            {
                var notificationType = NotificationType.Create(
                    notifEntity.notificationType.Id,
                    notifEntity.notificationType.notificationCategory,
                    notifEntity.notificationType.requiredClubMemberRole).notificationType;

                var notification = Notification.Create(
                    notifEntity.Id, 
                    notificationType, 
                    notifEntity.isChecked, 
                    notifEntity.Text, 
                    notifEntity.LinkedURL).notification;

                notification.CreatorId = notifEntity.senderId;
                notifications.Add(notification);
            }
            return  notifications;
        }

        public async Task Check(Guid NotificationId)
        {
            var notif = await _context.Notifications.FindAsync(NotificationId);
            if (notif == null) throw new Exception("Notification wasn't found!");
            
            notif.isChecked = true;
            notif.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

    }
}
