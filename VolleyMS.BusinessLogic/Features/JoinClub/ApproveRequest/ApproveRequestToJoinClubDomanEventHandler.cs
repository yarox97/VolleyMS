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
        private readonly IEmailService _emailSerivce;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveRequestToJoinClubDomainEventHandler(
            INotificationService notificationService,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork
            )
        {
            _notificationService = notificationService;
            _userRepository = userRepository;
            _emailSerivce = emailService;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(JoinClubApprovedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // Check if user exists
            var requestorUser = await _userRepository.GetByIdAsync(domainEvent.userId);

            if (requestorUser is null)
                throw new DomainException(DomainErrors.User.UserNotFound);

            // Create notification request
            var notificationRequest = new NotificationRequest
            {
                RequiredClubMemberRole = null,
                NotificationCategory = NotificationCategory.ClubJoinApproved, 
                Text = $"Your request to join club '{domainEvent.clubName}' was approved.",
                LinkedURL = $"/clubs/{domainEvent.clubId}",
                ClubId = domainEvent.clubId,
                ClubName = domainEvent.clubName
            };

            // Create payload
            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, domainEvent.responserId);

            if (payloadResult.IsFailure)
                throw new DomainException(payloadResult.Error);

            // Get sender info
            var sender = await _userRepository.GetByIdAsync(domainEvent.responserId);

            // Create notification
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

            // send notification on site
            var notification = notificationResult.Value;
            var sendResult = notification.Send(new List<User> { requestorUser });

            if (sendResult.IsFailure)
                throw new DomainException(sendResult.Error);

            // Save notification
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // send email 
            string subject = "Your request to join the club has been approved!";
            string body = $"<p>Congratulations! Your request to join the club '<b>{domainEvent.clubName}</b>' has been approved.</p>" +
                        $"<p>You can now access the club and participate in its activities.</p>" +
                        $"<p><a href='https://yourvolleymsapp.com/clubs/{domainEvent.clubId}'>Click here to visit the club page</a></p>" +
                        "<p>We look forward to seeing you active in the club!</p>" +
                        "<p>Best regards,<br/>VolleyMS Team</p>";

            await _emailSerivce.SendEmailAsync(requestorUser.Email,subject, body);
        }
    }
}
