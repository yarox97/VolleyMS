using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Entities;
using VolleyMS.Core.Models;
using VolleyMS.Core.Requests;
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

        public async Task<IList<Guid>> SendNotification(NotificationRequest notificationRequest, Guid senderId)
        {
            List<Guid> notifIds = new List<Guid>();

            for(int i = 0; i < notificationRequest.Receivers.Count(); ++i) 
            {
                var notifTypeTuple = NotificationType.Create(
                    Guid.NewGuid(),
                    notificationRequest.NotificationCategory,
                    notificationRequest.RequiredClubMemberRole);

                if (notifTypeTuple.error != string.Empty)
                    throw new Exception(notifTypeTuple.error);

                var notifTuple = Notification.Create(
                    Guid.NewGuid(),
                    notifTypeTuple.notificationType,
                    notificationRequest.IsChecked,
                    notificationRequest.Text,
                    notificationRequest.LinkedURL);

                if (notifTuple.error != string.Empty)
                    throw new Exception(notifTypeTuple.error);

                notifIds.Add(await _notificationRepository.Create(notifTuple.notification, new List<Guid> { notificationRequest.Receivers[i] }, senderId));
            }
            return notifIds;
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
