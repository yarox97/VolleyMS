using MediatR;
using VolleyMS.Core.DomainEvents.ClubMembers;
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
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public RequestToJoinClubDomainEventHandler(
            INotificationService notificationService,
            IClubRepository clubRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            INotificationRepository notificationRepository)
        {
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _clubRepository = clubRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            JoinClubRequestSentDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            // Get sender (requestor)
            var sender = await _userRepository.GetByIdAsync(domainEvent.requestorId);
            if (sender == null)
            {
                // LOGGING
                return; 
            }

            // Create notification request
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

            // Create payload
            var payloadResult =
                _notificationService.HandleCategorizedPayload(
                    notificationRequest,
                    domainEvent.requestorId);

            if (payloadResult.IsFailure)
                throw new DomainException(payloadResult.Error);

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

            var notification = notificationResult.Value;

            // Get receivers - club admins
            var clubMembers = await _clubRepository.GetUsersByRole(
                domainEvent.clubId,
                ClubMemberRole.Creator,
                ClubMemberRole.President,
                ClubMemberRole.Coach);

            // Send on site notifications to club admins
            var sendResult = notification.Send(clubMembers);

            if (sendResult.IsFailure)
                throw new DomainException(sendResult.Error);

            // Send email notifications to club admins
            var emails = clubMembers
                .Select(cm => cm.Email)
                .Where(email => !string.IsNullOrWhiteSpace(email))
                .ToList();

            string subject = $"New request to join your club '{domainEvent.clubName}'";
            string body =
                $"<p>Hello,</p>" +
                $"<p>You have received a new request to join the club '<b>{domainEvent.clubName}</b>'.</p>" +
                $"<p><strong>Requestor Details:</strong><br/>" +
                $"Name: {domainEvent.requestorName + " " + domainEvent.requestorSurname}<br/>" +
                $"Email: {sender.Email}</p>" +
                $"<p>Please review this request to approve or reject it.</p>" +
                $"<p><a href='https://yourvolleymsapp.com/clubs/{domainEvent.clubId}/requests'>Click here to manage requests</a></p>" +
                $"<p>Best regards,<br/>VolleyMS Team</p>";

            await _emailService.SendEmailsAsync(emails, subject, body);

            // Save notification
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
