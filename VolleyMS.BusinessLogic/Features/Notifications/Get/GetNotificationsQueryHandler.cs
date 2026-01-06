using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Get
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Result<PaginatedResult<NotificationDto>>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<PaginatedResult<NotificationDto>>> Handle(GetNotificationsQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(query.userId);
            if (user == null)
                return Result.Failure<PaginatedResult<NotificationDto>>(DomainErrors.User.UserNotFound);

            var validPageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var validPageSize = query.PageSize < 1 ? 10 : query.PageSize;

            var (notifications, totalCount) = await _notificationRepository.GetUserNotificationsPaged(
                user.Id,
                validPageNumber,
                validPageSize,
                cancellationToken);

            var dtos = notifications.Select(un => new NotificationDto
            {
                Id = un.Notification.Id,
                NotificationCategory = un.Notification.Category,
                Text = un.Notification.Text,
                LinkedURL = un.Notification.LinkedURL,
                Payload = un.Notification.Payload,
                CreatedAt = un.Notification.CreatedAt,
                IsChecked = un.IsChecked
            });

            var result = new PaginatedResult<NotificationDto>(dtos, totalCount, validPageNumber, validPageSize);

            return Result.Success(result);
        }
    }
}