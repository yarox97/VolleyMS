using MediatR;
using VolleyMS.Core.DomainEvents.Contracts;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Features.Contracts.Assign
{
    public sealed class ContractAssignedDomainEventHandler : INotificationHandler<ContractAssignedDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository; 
        private readonly IUnitOfWork _unitOfWork;
        public ContractAssignedDomainEventHandler(
            INotificationService notificationService, 
            INotificationRepository notificationRepository, 
            IUserRepository userRepository, 
            IClubRepository clubRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _notificationService = notificationService;
            _userRepository = userRepository;
            _clubRepository = clubRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ContractAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // Verify user existence
            var user = await _userRepository.GetByIdAsync(domainEvent.userId);
            if (user is null) return;
            //    Logging

            // Verify assigner existence
            var assigner = await _userRepository.GetByIdAsync(domainEvent.assignerId);
            if (assigner is null) return;
            //    Logging

            // Verify club existence
            var club = await _clubRepository.GetByIdAsync(domainEvent.clubId);
            if (club is null) return;
            //    Logging

            // Create notification
            var notificationRequest = new NotificationRequest
            { 
                RequiredClubMemberRole = null,
                NotificationCategory = NotificationCategory.ContractAssigned,
                Text = $"You have been assigned a new contract in club '{club.Name}'.",
                LinkedURL = $"/user/my-contracts",
                ClubId = club.Id,
                ClubName = club.Name,
                SenderName = assigner.Name,
                SenderSurname = assigner.Surname
            };

            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, domainEvent.assignerId);
            if (payloadResult.IsFailure)
                return; // Logging

            var notificationResult = Notification.Create(
                notificationRequest.RequiredClubMemberRole,
                notificationRequest.NotificationCategory,
                notificationRequest.Text,
                notificationRequest.LinkedURL,
                payloadResult.Value,
                assigner
                );

            // Send notification
            if (notificationResult.IsFailure)
                return; // Logging

            var notif = notificationResult.Value;
            var notifSendResult = notif.Send(new List<User> { user });

            if(notifSendResult.IsFailure)
                return; // Logging

            await _notificationRepository.AddAsync(notif);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
