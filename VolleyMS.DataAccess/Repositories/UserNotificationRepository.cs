using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;

namespace VolleyMS.DataAccess.Repositories
{
    public class UserNotificationRepository : GenericRepository<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }

        public async Task<UserNotification?> GetByIds(Guid userId, Guid notificationId)
        {
            return await _volleyMsDbContext.UserNotifications
                .Where(un => un.UserId == userId && un.NotificationId == notificationId)
                .FirstOrDefaultAsync();
        }
    }
}
