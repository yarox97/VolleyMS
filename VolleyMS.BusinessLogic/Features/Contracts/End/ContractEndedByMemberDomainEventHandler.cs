using MediatR;
using VolleyMS.Core.DomainEvents.Contracts;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Features.Contracts.End
{
    public sealed class ContractEndedByMemberDomainEventHandler : INotificationHandler<ContractEndedByMemberDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ContractEndedByMemberDomainEventHandler(
            INotificationService notificationService,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IClubRepository clubRepository,
            IContractRepository contractRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _notificationService = notificationService;
            _userRepository = userRepository;
            _clubRepository = clubRepository;
            _contractRepository = contractRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task Handle(ContractEndedByMemberDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // Verify user existence
            var user = await _userRepository.GetByIdAsync(domainEvent.userId);
            if (user == null) return;
            // LOGGING

            // Verify club existence
            var club = await _clubRepository.GetByIdAsync(domainEvent.clubId);
            if (club == null) return;
            // LOGGING

            // Verify contract existence
            var contract = await _contractRepository.GetByIdAsync(domainEvent.contractId);
            if (contract == null) return;
            // LOGGING

            // Get resipients
            var aligedUsers = await _clubRepository.GetUsersByRole(domainEvent.clubId, ClubRoleConfig.GetRolesByPermission(ClubPermission.ViewContracts).ToArray());

            // Exclude the user who ended the contract
            var recipients = aligedUsers.Where(u => u.Id != domainEvent.userId).ToList();

            // Create notification
            var notificationRequest = new NotificationRequest
            {
                RequiredClubMemberRole = ClubRoleConfig.GetRolesByPermission(ClubPermission.ViewContracts),
                Text = $"Member '{user.Name} {user.Surname}' has ended their contract in club '{club.Name}'.",
                LinkedURL = $"/clubs/{club.Id}/contracts/{contract.Id}",
                ClubName = club.Name,
                SenderName = user.Name,
                SenderSurname = user.Surname,
                NotificationCategory = NotificationCategory.ContractEndedByUser
            };

            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, domainEvent.userId);
            if (payloadResult.IsFailure)
                return;
            // LOGGING

            var notificationResult = Notification.Create(
                notificationRequest.RequiredClubMemberRole,
                notificationRequest.NotificationCategory,
                notificationRequest.Text,
                notificationRequest.LinkedURL,
                payloadResult.Value,
                user
                );

            if (notificationResult.IsFailure)
                return;

            // Send notification
            var notification = notificationResult.Value;
            var notificationSendResult = notification.Send(aligedUsers);

            if (notificationSendResult.IsFailure)
                return;
            // LOGGING

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
