using VolleyMS.Core.Models;

namespace VolleyMS.Core.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        public Task<IEnumerable<UserNotification>> GetUserNotifications(Guid userId, CancellationToken cancellationToken);
    }
}
