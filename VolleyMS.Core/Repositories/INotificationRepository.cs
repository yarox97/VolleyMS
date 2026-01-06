using VolleyMS.Core.Models;

namespace VolleyMS.Core.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<(IEnumerable<UserNotification> Items, int TotalCount)> GetUserNotificationsPaged(
        Guid userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    }
}
