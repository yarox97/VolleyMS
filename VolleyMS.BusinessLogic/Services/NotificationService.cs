using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Entities;
using VolleyMS.DataAccess.Repositories;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Services
{
    public class NotificationService
    {
        private readonly NotificationRepository _notificationRepository;
        public NotificationService(NotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification> SendNotification(Notification notification, string Error, Guid senderId, IList<Guid> receivers)
        {
            if (Error != string.Empty)
            {
                throw new Exception(Error);
            }

            return await _notificationRepository.Create(notification, receivers, senderId);
        }

        public async Task<IList<Notification>> GetNotifications(Guid userId)
        {
            var notifications = await _notificationRepository.GetUserNotifications(userId);
            return notifications == null ? new List<Notification>() : notifications;  
        }

        public async Task Check(Guid notifId)
        {
            await _notificationRepository.Check(notifId);
        }  
    }
}
