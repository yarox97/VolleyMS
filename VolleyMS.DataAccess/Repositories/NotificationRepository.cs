using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Exceptions;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;
using VolleyMS.DataAccess.Mappers;
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
        public async Task<Guid> Create(Notification notification, IList<Guid> receivers, Guid? senderId)
        {
            if (notification == null)
            {
                throw new Exception("Notification is empty!"); // TO CHANGE EXCEPTION TYPE
            }

            // get sender entity
            var senderEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == senderId); //?? throw new UserNotFoundDomainException("Sender not found!"); // TO CHANGE EXCEPTION TYPE

            // get receiver entities
            var receiversEntities = await _context.Users
               .Where(u => receivers.Contains(u.Id))
               .ToListAsync();

            if (receiversEntities.Count < 1) throw new UserNotFoundDomainException("No recievers found!"); // TO CHANGE EXCEPTION TYPE

            // create notification entity
            var NotificationEntity = NotificationMapper.ToEntity(notification, senderId);

            // Create notification for each receiver
            NotificationEntity.UserNotifications = receiversEntities
                .Select(r => new UserNotificationsEntity
                {
                    NotificationId = NotificationEntity.Id,
                    UserId = r.Id,
                    IsChecked = false
                })
                .ToList();

            await _context.Notifications.AddAsync(NotificationEntity);
            await _context.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<Guid> DeleteUserNotification(Guid notificationId, Guid userId)
        {
            await _context.UserNotifications
                .Where(un => un.NotificationId == notificationId && un.UserId == userId)
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();
            return notificationId;
        }

        public async Task<Guid> DeleteCascade(Guid notificationId)
        {
            await _context.Notifications
                .Where(n => n.Id == notificationId)
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();
            return notificationId;
        }

        public async Task<IList<UserNotificationsEntity>> GetUserNotifications(Guid userId)
        {
            var NotificationEntities = await _context.UserNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId)
                .ToListAsync();

            List<UserNotificationsEntity> Notifications = new List<UserNotificationsEntity>();

            if(NotificationEntities == null || NotificationEntities.Count < 1)
                return Notifications;

            foreach (var NotificationEntity in NotificationEntities)
            {
                Notifications.Add(NotificationEntity);
            }

            return Notifications;
        }

        public async Task Check(Guid NotificationId, string userName)
        {
            var NotificationEntity = await _context.UserNotifications
                .FirstOrDefaultAsync(un => un.NotificationId == NotificationId && un.User.UserName == userName);
            
            if (NotificationEntity == null) 
                throw new Exception("Notification wasn't found!");

            NotificationEntity.IsChecked = true;
            NotificationEntity.UpdatedAt = DateTime.UtcNow; // TODO domain event for update
            await _context.SaveChangesAsync();
        }

        
    }
}
