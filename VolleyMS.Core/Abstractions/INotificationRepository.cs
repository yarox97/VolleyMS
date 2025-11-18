using VolleyMS.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.DataAccess.Repositories
{
    public interface INotificationRepository
    {
        public Task<Guid> Create(Notification notification, IList<Guid> receivers, Guid senderId);
        public Task Delete(Guid Id);
        public Task Check(Guid NotificationId);
    }
}