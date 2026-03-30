using MediatR;
using VolleyMS.Core.DomainEvents.Tasks;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Exceptions;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;

namespace VolleyMS.BusinessLogic.Features.Tasks.UpdateDetails
{
    public class TaskUpdatedDomainEventHandler : INotificationHandler<TaskUpdatedDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaskUpdatedDomainEventHandler(
            INotificationService notificationService,
            INotificationRepository notificationRepository,
            ITaskRepository taskRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async System.Threading.Tasks.Task Handle(TaskUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // get task details
            var task = await _taskRepository.GetByIdWithDetailsAsync(domainEvent.TaskId, cancellationToken);
            if (task is null) throw new DomainException(DomainErrors.Task.TaskNotFound);

            // get receivers 
            var receivers = task.Receivers.Select(r => r.User).ToList();
            if (!receivers.Any()) return;

            var assigner = task.Sender;

            var notificationRequest = new NotificationRequest
            {
                NotificationCategory = NotificationCategory.TaskModified,
                Text = $"Task details '{task.Title}' were changed.",
                LinkedURL = $"/tasks/{task.Id}",

                OldStartsFrom = domainEvent.OldStartDate,
                OldEndsBy = domainEvent.OldEndDate,
                OldTaskPriority = domainEvent.OldTaskPriority.ToString(),
                OldTaskTitle = domainEvent.OldTitle,
                OldTaskDescription = domainEvent.OldDescription,

                StartsFrom = task.TaskSchedule.StartDate,
                EndsBy = task.TaskSchedule.EndDate,
                TaskPriority = task.TaskPriority.ToString(),
                TaskTitle = task.Title,
                TaskDescription = task.Description,

                ClubName = task.Club.Name,
                SenderName = assigner.Name,
                SenderSurname = assigner.Surname,
                TaskId = task.Id
            };

            // create payload 
            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, assigner.Id);
            if (payloadResult.IsFailure) throw new DomainException(payloadResult.Error);

            // create notification
            var notificationResult = Notification.Create(
                null, notificationRequest.NotificationCategory, notificationRequest.Text,
                notificationRequest.LinkedURL, payloadResult.Value, assigner);

            if (notificationResult.IsFailure) throw new DomainException(notificationResult.Error);

            // send notification
            var sendResult = notificationResult.Value.Send(receivers);
            if (sendResult.IsFailure) throw new DomainException(sendResult.Error);

            await _notificationRepository.AddAsync(notificationResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
