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

namespace VolleyMS.BusinessLogic.Features.JoinClub.RejectRequest
{
    public class RejectRequestToJoinClubDomainEventHandler : INotificationHandler<JoinClubRejectedDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public RejectRequestToJoinClubDomainEventHandler(
            INotificationService notificationService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            INotificationRepository notificationRepository)
        {
            _notificationService = notificationService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }
        public async Task Handle(JoinClubRejectedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // Get requestor user
            var requestorUser = await _userRepository.GetByIdAsync(domainEvent.userId);

            if (requestorUser is null)
                throw new DomainException("User not found in: reject join club");

            // Create notification request
            var notificationRequest = new NotificationRequest
            {
                RequiredClubMemberRole = null,
                NotificationCategory = NotificationCategory.ClubJoinRejected,
                Text = $"Your request to join club '{domainEvent.clubName}' was rejected.",
                LinkedURL = "/clubs",
                ClubId = domainEvent.clubId,
                ClubName = domainEvent.clubName
            };

            // Create payload
            var payload = new JoinClubRequestRejectedPayLoad(domainEvent.clubId, domainEvent.clubName);
            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, domainEvent.requestorId);

            if (payloadResult.IsFailure)
                throw new DomainException(payloadResult.Error);

            // Get responser info
            var responser = await _userRepository.GetByIdAsync(domainEvent.responserId);

            // Create notification
            var notificationResult = Notification.Create(
                notificationRequest.RequiredClubMemberRole,
                notificationRequest.NotificationCategory,
                notificationRequest.Text,
                notificationRequest.LinkedURL,
                payloadResult.Value,
                responser);

            if (notificationResult.IsFailure)
                throw new DomainException(notificationResult.Error);

            var notification = notificationResult.Value;


            // Send on site notification
            var sendResult = notification.Send(new List<User> { requestorUser });

            if (sendResult.IsFailure)
                throw new DomainException(sendResult.Error);

            // Send email
            string subject = $"Your request to join the club { domainEvent.clubName } has been rejected.";
            string body = $"Hello { requestorUser.Name },\n\n" +
                          $"We regret to inform you that your request to join the club '{ domainEvent.clubName }' has been rejected.\n\n" +
                          "If you have any questions or would like more information, please feel free to contact the club administration.\n\n" +
                          "Best regards,\n" +
                          "VolleyMS Team";

            await _emailService.SendEmailAsync(requestorUser.Email, subject, body);

            // Save notification
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}