using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;


namespace VolleyMS.DataAccess.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }

        public async Task<Guid> DeleteUserNotification(Guid notificationId, Guid userId)
        {
            await _volleyMsDbContext.UserNotifications
                .Where(un => un.NotificationId == notificationId && un.UserId == userId)
                .ExecuteDeleteAsync();

            await _volleyMsDbContext.SaveChangesAsync();
            return notificationId;
        }

        public async Task<IEnumerable<UserNotification>> GetUserNotifications(Guid userId, CancellationToken cancellationToken)
        {
            var userNotifications = await _volleyMsDbContext.UserNotifications
                .Where(un => un.UserId == userId)
                .Include(un => un.Notification)
                .ToListAsync(cancellationToken);

            return userNotifications;
        }
    }
}
