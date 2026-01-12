using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Get
{
    public class GetNotificationQueryHandler : IRequestHandler<GetNotificationQuery, Result<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;

        public GetNotificationQueryHandler(INotificationRepository notificationRepository, IUserRepository userRepository, IUserNotificationRepository userNotificationRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _userNotificationRepository = userNotificationRepository;
        }

        public async Task<Result<NotificationDto>> Handle(GetNotificationQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId);
            if (user == null)
                return Result.Failure<NotificationDto>(DomainErrors.User.UserNotFound);

            var userNotification = await _userNotificationRepository.GetByIds(query.UserId, query.NotificationId);
            if(userNotification == null)
                return Result.Failure<NotificationDto>(DomainErrors.Notification.NotificationNotFound);

            var notification = await _notificationRepository.GetByIdAsync(userNotification.NotificationId);
            if(notification == null)
                return Result.Failure<NotificationDto>(DomainErrors.Notification.NotificationNotFound);


            var dto = new NotificationDto
            {
                Id = notification.Id,
                NotificationCategory = notification.Category,
                Text = notification.Text,
                LinkedURL = notification.LinkedURL,
                Payload = notification.Payload,
                CreatedAt = notification.CreatedAt,
                IsChecked = userNotification.IsChecked,
                SenderId = notification.Sender?.Id,
                SenderUsername = notification.Sender?.UserName
            };

            return Result.Success(dto);
        }
    }
}
