using MediatR;
using VolleyMS.Core.DomainEvents;
using VolleyMS.Core.Exceptions;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using Notification = VolleyMS.Core.Models.Notification;

namespace VolleyMS.BusinessLogic.Features.JoinClub.RequestToJoinClub
{
    public sealed class RequestToJoinClubDomainEventHandler
    : INotificationHandler<JoinClubRequestSentDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestToJoinClubDomainEventHandler(
            INotificationService notificationService,
            IClubRepository clubRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            INotificationRepository notificationRepository)
        {
            _notificationService = notificationService;
            _clubRepository = clubRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(
            JoinClubRequestSentDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            var sender = await _userRepository.GetByIdAsync(domainEvent.requestorId);
            if (sender == null)
            {
                // LOGGING
                return; 
            }

            var notificationRequest = new NotificationRequest
            {
                RequiredClubMemberRole = new List<ClubMemberRole>
            {
                ClubMemberRole.Creator,
                ClubMemberRole.President,
                ClubMemberRole.Coach
            },
                NotificationCategory = NotificationCategory.ClubJoinRequest,
                Text = $"New request to join club '{domainEvent.clubName}' from user '{domainEvent.requestorName}'.",
                LinkedURL = $"/clubs/{domainEvent.clubId}/join-requests",
                ClubId = domainEvent.clubId,
                SenderName = domainEvent.requestorName,
                SenderSurname = domainEvent.requestorSurname,
                ClubName = domainEvent.clubName
            };

            var payloadResult =
                _notificationService.HandleCategorizedPayload(
                    notificationRequest,
                    domainEvent.requestorId);

            if (payloadResult.IsFailure)
                throw new DomainException(payloadResult.Error);

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

            var clubMembers = await _clubRepository.GetUsersByRole(
                domainEvent.clubId,
                ClubMemberRole.Creator,
                ClubMemberRole.President,
                ClubMemberRole.Coach);

            var sendResult = notification.Send(clubMembers);

            if (sendResult.IsFailure)
                throw new DomainException(sendResult.Error);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
