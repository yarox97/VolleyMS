using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized;
using VolleyMS.Core.DomainEvents;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.RejectRequest
{
    public class RejectRequestToJoinClubDomainEventHandler
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RejectRequestToJoinClubDomainEventHandler(
            INotificationService notificationService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            INotificationRepository notificationRepository)
        {
            _notificationService = notificationService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
        }
        public async Task<Result> Handle(JoinClubRejectedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var requestorUser = await _userRepository.GetByIdAsync(domainEvent.requestorId);

            if (requestorUser is null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            var notificationRequest = new NotificationRequest
            {
                RequiredClubMemberRole = null,
                NotificationCategory = NotificationCategory.ClubJoinRejected,
                Text = $"Your request to join club '{domainEvent.clubName}' was rejected.",
                LinkedURL = "/clubs"
            };

            var payload = new JoinClubRequestRejectedPayLoad(domainEvent.clubId, domainEvent.clubName);

            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, domainEvent.requestorId);

            if (payloadResult.IsFailure)
                return Result.Failure(payloadResult.Error);

            var notificationResult = Notification.Create(
                notificationRequest.RequiredClubMemberRole,
                notificationRequest.NotificationCategory,
                notificationRequest.Text,
                notificationRequest.LinkedURL,
                payloadResult.Value);

            if (notificationResult.IsFailure)
                return Result.Failure(notificationResult.Error);

            var notification = notificationResult.Value;

            var sendResult = notification.Send(new List<User> { requestorUser });

            if (sendResult.IsFailure)
                return Result.Failure(sendResult.Error);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}