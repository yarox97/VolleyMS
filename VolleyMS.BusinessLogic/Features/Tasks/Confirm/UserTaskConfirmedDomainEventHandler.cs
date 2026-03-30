using MediatR;
using System.Globalization;
using VolleyMS.Core.DomainEvents.Tasks;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Exceptions;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;

namespace VolleyMS.BusinessLogic.Features.Tasks.Confirm
{
    public class UserTaskConfirmedDomainEventHandler : INotificationHandler<UserTaskConfirmedDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserTaskRepository _userTaskRepository; 
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public UserTaskConfirmedDomainEventHandler(
            INotificationService notificationService,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IUserTaskRepository userTaskRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _userTaskRepository = userTaskRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }
        public async System.Threading.Tasks.Task Handle(UserTaskConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var userTask = await _userTaskRepository.GetByIdWithDetailsAsync(domainEvent.UserTaskId, cancellationToken);

            if (userTask is null)
                throw new DomainException(DomainErrors.Task.TaskNotFound);

            var assignee = userTask.User;
            var confirmatorId = domainEvent.ConfirmatorId; 
            var confirmator = await _userRepository.GetByIdAsync(confirmatorId) ?? assignee;

            var notificationRequest = new NotificationRequest
            {
                NotificationCategory = NotificationCategory.TaskConfirmed,
                Text = $"Your task '{userTask.Task.Title}' was checked and confirmed by {confirmator.Name + " " + confirmator.Surname}" +
                $" If there are any penalty/bonus for this task then your monthly payslip was already recalculated.",
                LinkedURL = $"/tasks/{userTask.TaskId}",

                // Поля для TaskConfirmedPayload
                StartsFrom = userTask.Task.TaskSchedule.StartDate + userTask.Task.TaskSchedule.StartTime,
                EndsBy = userTask.Task.TaskSchedule.EndDate + userTask.Task.TaskSchedule.EndTime,
                ConfirmedAt = DateTime.UtcNow,
                TaskPriority = userTask.Task.TaskPriority.ToString(),
                TaskTitle = userTask.Task.Title,
                TaskDescription = userTask.Task.Description,
                TaskPenaltyValue = userTask.Task.PenaltyAmount?.ToString() ?? "0",
                TaskPenaltyType = userTask.Task.PenaltyType.ToString(),
                ClubName = userTask.Task.Club?.Name ?? "Unknown club",
                SenderName = confirmator.Name,
                SenderSurname = confirmator.Surname,
                TaskId = userTask.TaskId
            };

            // Create payload
            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, confirmator.Id);
            if (payloadResult.IsFailure) throw new DomainException(payloadResult.Error);

            var notificationResult = Notification.Create(
                null, notificationRequest.NotificationCategory, notificationRequest.Text,
                notificationRequest.LinkedURL, payloadResult.Value, confirmator);

            if (notificationResult.IsFailure) throw new DomainException(notificationResult.Error);

            // send on-site notification
            var sendResult = notificationResult.Value.Send(new List<User> { assignee }); 
            if (sendResult.IsFailure) throw new DomainException(sendResult.Error);

            await _notificationRepository.AddAsync(notificationResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
