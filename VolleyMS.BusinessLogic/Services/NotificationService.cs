using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.BusinessLogic.Features.Notifications;
using VolleyMS.Core.Shared;
using VolleyMS.Core.Requests;

namespace VolleyMS.BusinessLogic.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public Result<string> HandleCategorizedPayload(NotificationRequest notificationRequest, Guid? senderId)
        {
            var payload = PayLoadFactory.CreatePayLoad(notificationRequest, senderId); // ТУТ НАДО КИДАТЬ ВСЕ ОШИБКИ КОРОЧЕ ЧТОБЫ ОНИ ПРОПИХИВАЛИСЬ ВЫШЕ

            return payload ?? Result.Failure<string>(DomainErrors.Notification.ErrorCreatingPayload);
        }  
    }
}
