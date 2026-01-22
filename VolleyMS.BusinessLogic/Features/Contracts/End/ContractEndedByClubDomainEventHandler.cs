using MediatR;
using VolleyMS.Core.DomainEvents;
using VolleyMS.Core.DomainEvents.Contracts;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Features.Contracts.End
{
    public sealed class ContractEndedByClubDomainEventHandler : INotificationHandler<ContractEndedByAssignerDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ContractEndedByClubDomainEventHandler(
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
        public async Task Handle(ContractEndedByAssignerDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // Verify user existence
            var assigner = await _userRepository.GetByIdAsync(domainEvent.assignerId);
            if (assigner == null) return;
            // LOGGING

            // verify user existence
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

            // Create notification
            var notificationRequest = new NotificationRequest
            {
                RequiredClubMemberRole = ClubRoleConfig.GetRolesByPermission(ClubPermission.ViewContracts),
                Text = $"Your contract in club '{club.Name}' has been ended by '{assigner.Name} {assigner.Surname}'.",
                LinkedURL = $"/user/my-contracts/{contract.Id}",
                ClubName = club.Name,
                SenderName = assigner.Name,
                SenderSurname = assigner.Surname,
                NotificationCategory = NotificationCategory.ContractEndedByClub
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
                assigner
                );

            if (notificationResult.IsFailure)
                return;

            // Send notification
            var notification = notificationResult.Value;
            var notificationSendResult = notification.Send(new List<User> { user });

            if (notificationSendResult.IsFailure)
                return;
            // LOGGING

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
