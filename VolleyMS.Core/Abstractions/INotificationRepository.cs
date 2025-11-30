using VolleyMS.Core.Models;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.DataAccess.Repositories
{
    public interface INotificationRepository
    {
        public Task<Guid> Create(Notification notification, IList<Guid> receivers, Guid? senderId);
        public Task<Guid> DeleteCascade(Guid notificationId);
        public Task<Guid> DeleteUserNotification(Guid notificationId, Guid userId);
        public Task Check(Guid NotificationId, string userName);
    }
}