using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Notifications.Check
{
    public sealed class CheckNotificationCommandHandler : IRequestHandler<CheckNotificationCommand, Result>
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CheckNotificationCommandHandler(IUserNotificationRepository userNotificationRepository, IUserRepository userRepository, INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _userNotificationRepository = userNotificationRepository;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CheckNotificationCommand request, CancellationToken cancellationToken)
        {
            var User = await _userRepository.GetByIdAsync(request.userId);
            if (User == null) 
                    return Result.Failure(DomainErrors.User.UserNotFound);

            var Notification = await _notificationRepository.GetByIdAsync(request.notificationId);
            if (Notification == null)
                return Result.Failure(Error.NullValue);

            var UserNotification = _userNotificationRepository.GetByIds(request.userId, request.notificationId).Result;
            if (UserNotification == null)
                return Result.Failure(Error.NullValue);

            UserNotification.Check();

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
