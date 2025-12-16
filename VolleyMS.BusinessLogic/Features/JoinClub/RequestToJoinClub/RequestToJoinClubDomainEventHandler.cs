using VolleyMS.Core.DomainEvents;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;
using Notification = VolleyMS.Core.Models.Notification;

namespace VolleyMS.BusinessLogic.Features.JoinClub.RequestToJoinClub
{
    public class RequestToJoinClubDomainEventHandler
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RequestToJoinClubDomainEventHandler(INotificationService notificationSerivce, IClubRepository clubRepository, IUnitOfWork unitOfWork, INotificationRepository notificationRepository)
        {
            _notificationService = notificationSerivce;
            _clubRepository = clubRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
        }
        public async Task<Result> Handle(JoinClubRequestSentDomainEvent domainEvent, CancellationToken cancellationToken)
        {
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
            };

            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, domainEvent.requestorId);

            if (payloadResult.IsFailure)
                return Result.Failure<Notification>(payloadResult.Error);

            var notificationResult = Notification.Create(
                notificationRequest.RequiredClubMemberRole,
                notificationRequest.NotificationCategory,
                notificationRequest.Text,
                notificationRequest.LinkedURL,
                payloadResult.Value);

            if(notificationResult.IsFailure)
                return Result.Failure<Notification>(notificationResult.Error);

            var notification = notificationResult.Value;

            var clubMembers = await _clubRepository.GetUsersByRole(domainEvent.clubId,
                ClubMemberRole.Creator,
                ClubMemberRole.President,
                ClubMemberRole.Coach);


            var sendResult = notification.Send(clubMembers);

            if (sendResult.IsFailure)
                return Result.Failure<Notification>(sendResult.Error);

            await _notificationRepository.AddAsync(notification);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
