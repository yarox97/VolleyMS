using MediatR;
using VolleyMS.Core.DomainEvents.ClubMembers;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Features.ClubMembers.Kick
{
    public sealed class ClubMemberKickedDomainEventHandler : INotificationHandler<ClubMemberKickedDomainEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserClubRepository _userClubRepository;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly IClubRepository _clubRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationRepository _notificationRepository;

        public ClubMemberKickedDomainEventHandler(IUserRepository userRepository, IUserClubRepository userClubRepository, INotificationService notificationService, IEmailService emailService, IUnitOfWork unitOfWork, IClubRepository clubRepository, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _userClubRepository = userClubRepository;
            _notificationService = notificationService;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _clubRepository = clubRepository;
            _notificationRepository = notificationRepository;
        }
        public async Task Handle(ClubMemberKickedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // Check user existence
            var user = await _userRepository.GetByIdAsync(domainEvent.userId);
            if (user is null)
                return;
            //LOGG 

            // Check club existence
            var club = await _clubRepository.GetByIdAsync(domainEvent.clubId);
            if (club is null)
                return;
            //LOGG

            // Create notification request 
            var notificationRequest = new NotificationRequest
            {
                Text = $"You have been kicked from the club '{club.Name}'.",
                NotificationCategory = NotificationCategory.KickedFromClubByClub,
                Receivers = new List<Guid> { user.Id },
                RequiredClubMemberRole = null,
                ClubId = club.Id,
                ClubName = club.Name
            };

            // create payload

            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, domainEvent.clubId);
            if (payloadResult.IsFailure)
                return;
            // LOGG

            // Create notification
            var notificationResult = Notification.Create(
                notificationRequest.RequiredClubMemberRole,
                notificationRequest.NotificationCategory,
                notificationRequest.Text,
                notificationRequest.LinkedURL,
                payloadResult.Value,
                null
                );

            if (notificationResult.IsFailure)
                return;
            // LOGG

            var notification = notificationResult.Value;

            // Send notification on site
            var notificationSendResult = notification.Send(new List<User> { user });
            if (notificationSendResult.IsFailure)
                return;

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send email notification
            string subject = $"You have been kicked from the club { club.Name }";
            string body = $"<p>We regret to inform you that you have been kicked from the club '<b>{club.Name}</b>'.</p>" +
                          $"<p>If you believe this was a mistake or have any questions, please contact the club administrator for more information.</p>" +
                          $"<p>Best regards,<br/>VolleyMS Team</p>";

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
    }
}
