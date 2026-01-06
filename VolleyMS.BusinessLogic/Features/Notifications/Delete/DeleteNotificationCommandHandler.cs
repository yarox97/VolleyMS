

using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Delete
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteNotificationCommandHandler(IUserRepository userRepository, INotificationRepository notificationRepository, IUserNotificationRepository userNotificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _userNotificationRepository = userNotificationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId);
            if (user == null) 
                return Result.Failure(DomainErrors.User.UserNotFound);

            var notification = await _notificationRepository.GetByIdAsync(request.notificationId);
            if(notification == null)
                return Result.Failure(DomainErrors.Notification.NotificationNotFound);

            var userNotification = await _userNotificationRepository.GetByIds(request.userId, request.notificationId);
            if(userNotification == null)
                return Result.Failure(DomainErrors.Notification.NotificationNotFound);

            await _userNotificationRepository.DeleteAsync(userNotification.Id);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
