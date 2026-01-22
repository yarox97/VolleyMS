using MediatR;
using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized;
using VolleyMS.Core.DomainEvents.ClubMembers;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Exceptions;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Features.JoinClub.ApproveRequest
{
    public class ApproveRequestToJoinClubDomainEventHandler : INotificationHandler<JoinClubApprovedDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveRequestToJoinClubDomainEventHandler(
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

        public async Task Handle(JoinClubApprovedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var requestorUser = await _userRepository.GetByIdAsync(domainEvent.userId);

            if (requestorUser is null)
                throw new DomainException(DomainErrors.User.UserNotFound);

            var notificationRequest = new NotificationRequest
            {
                RequiredClubMemberRole = null,
                NotificationCategory = NotificationCategory.ClubJoinApproved, 
                Text = $"Your request to join club '{domainEvent.clubName}' was approved.",
                LinkedURL = $"/clubs/{domainEvent.clubId}",
                ClubId = domainEvent.clubId,
                ClubName = domainEvent.clubName
            };

            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, domainEvent.responserId);

            if (payloadResult.IsFailure)
                throw new DomainException(payloadResult.Error);

            var sender = await _userRepository.GetByIdAsync(domainEvent.responserId);
            

            var notificationResult = Notification.Create(
                notificationRequest.RequiredClubMemberRole,
                notificationRequest.NotificationCategory,
                notificationRequest.Text,
                notificationRequest.LinkedURL,
                payloadResult.Value,
                sender
                );

            if (notificationResult.IsFailure)
                throw new DomainException(notificationResult.Error);

            var notification = notificationResult.Value;

            var sendResult = notification.Send(new List<User> { requestorUser });

            if (sendResult.IsFailure)
                throw new DomainException(sendResult.Error);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
