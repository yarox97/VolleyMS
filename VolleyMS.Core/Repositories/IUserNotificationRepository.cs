using VolleyMS.Core.Models;

namespace VolleyMS.Core.Repositories
{
    public interface IUserNotificationRepository : IGenericRepository<UserNotification>
    {
        public Task<UserNotification?> GetByIds(Guid userId, Guid notificationId);
    }
}
