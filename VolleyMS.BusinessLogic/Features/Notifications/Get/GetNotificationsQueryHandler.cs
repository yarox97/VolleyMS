using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Get
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Result<IEnumerable<NotificationDto>>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository, IUserRepository userRepository, IClubRepository clubRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _clubRepository = clubRepository;
        }
        public async Task<Result<IEnumerable<NotificationDto>>> Handle(GetNotificationsQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(query.userId);
            if (user == null)
                return Result.Failure<IEnumerable<NotificationDto>>(DomainErrors.User.UserNotFound);

            var notifications = await _notificationRepository.GetUserNotifications(user.Id, cancellationToken);

            return Result.Success(notifications.Select(un => new NotificationDto
            {
                Id = un.Notification.Id,
                NotificationCategory = un.Notification.Category,
                Text = un.Notification.Text,
                LinkedURL = un.Notification.LinkedURL,
                Payload = un.Notification.Payload,
                CreatedAt = un.Notification.CreatedAt,
                IsChecked = un.IsChecked
            }));
        }
    }
}
