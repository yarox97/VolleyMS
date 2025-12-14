//using VolleyMS.BusinessLogic.NotificationPayloads;
//using VolleyMS.Core.Models;
//using VolleyMS.Core.Requests;
//using VolleyMS.DataAccess.Repositories;
//using Task = System.Threading.Tasks.Task;

//namespace VolleyMS.BusinessLogic.Services
//{
//    public class NotificationService
//    {
//        private readonly NotificationRepository _notificationRepository;
//        public NotificationService(NotificationRepository notificationRepository)
//        {
//            _notificationRepository = notificationRepository;
//        }

//        private Notification HandleCategorizedNotification(NotificationRequest notificationRequest, Guid? senderId)
//        {
//            var payload = PayLoadFactory.CreatePayLoad(notificationRequest, senderId);

//            var notification = Notification.Create(
//                notificationRequest.RequiredClubMemberRole,
//                notificationRequest.NotificationCategory,
//                notificationRequest.Text,
//                notificationRequest.LinkedURL,
//                payload);

//            return notification;
//        }

//        public async Task<Guid> SendNotification(NotificationRequest NotificationRequest, Guid SenderId)
//        {
//            var CategorizedNotification = HandleCategorizedNotification(NotificationRequest, SenderId);

//            await _notificationRepository.Create(CategorizedNotification, NotificationRequest.Receivers, SenderId);

//            return CategorizedNotification.Id;
//        }

//        public async Task<IList<UserNotificationResult>> Get(Guid UserId)
//        {
//            var Notifications = await _notificationRepository.GetUserNotifications(UserId);
//            var UserNotificationResult = new List<UserNotificationResult>();
//            foreach (var Notification in Notifications)
//            {
//                var DomainNotification = NotificationMapper.ToDomain(Notification.Notification);
//                UserNotificationResult.Add(new UserNotificationResult(DomainNotification, Notification.IsChecked));
//            }
//            return UserNotificationResult;
//        }

//        public async Task Check(Guid NotificationId, string userName)
//        {
//            await _notificationRepository.Check(NotificationId, userName);
//        }

//        public async Task<Guid> DeleteUserNotification(Guid notificationId, Guid userId)
//        {
//            return await _notificationRepository.DeleteUserNotification(notificationId, userId);
//        }

//        public async Task<Guid> DeleteUsersNotifications(Guid notificationId)
//        {
//            return await _notificationRepository.DeleteCascade(notificationId);
//        }
//    }
//}
