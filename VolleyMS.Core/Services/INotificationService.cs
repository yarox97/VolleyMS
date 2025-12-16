using VolleyMS.Core.Models;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Services
{
    public interface INotificationService
    {
        public Result<string> HandleCategorizedPayload(NotificationRequest notificationRequest, Guid? senderId);
    }
}
