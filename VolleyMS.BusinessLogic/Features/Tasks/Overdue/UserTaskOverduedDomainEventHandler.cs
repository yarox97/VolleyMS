using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.DomainEvents.Tasks;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Exceptions;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using System.Globalization;

namespace VolleyMS.BusinessLogic.Features.Tasks.Overdue
{
    public class UserTaskOverduedDomainEventHandler : INotificationHandler<UserTaskOverdueDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserTaskOverduedDomainEventHandler(
            INotificationService notificationService,
            INotificationRepository notificationRepository,
            IUserTaskRepository userTaskRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _userTaskRepository = userTaskRepository;
            _unitOfWork = unitOfWork;
        }
        public async System.Threading.Tasks.Task Handle(UserTaskOverdueDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var userTask = await _userTaskRepository.GetByIdWithDetailsAsync(domainEvent.UserTaskId, cancellationToken);
            if (userTask is null) throw new DomainException(DomainErrors.Task.TaskNotFound);

            var assignee = userTask.User;

            var endDateTime = ((DateTime)userTask?.Task?.TaskSchedule?.EndDate + (TimeSpan)userTask?.Task?.TaskSchedule?.EndTime).ToString("g", CultureInfo.InvariantCulture); // to convert to a more readable format, e.g., "6/15/2024 2:30 PM"

            var notificationRequest = new NotificationRequest
            {
                NotificationCategory = NotificationCategory.TaskOverdue,
                Text = $"Task '{userTask.Task.Title}' in '{userTask.Task.Club.Name}' is overdue. " +
                $"The deadline was {endDateTime}.",
                LinkedURL = $"/tasks/{userTask.TaskId}",

                StartsFrom = userTask.Task.TaskSchedule.StartDate + userTask.Task.TaskSchedule.StartTime,
                EndsBy = userTask.Task.TaskSchedule.EndDate + userTask.Task.TaskSchedule.EndTime,
                TaskPriority = userTask.Task.TaskPriority.ToString(),
                TaskTitle = userTask.Task.Title,
                TaskDescription = userTask.Task.Description,
                ClubName = userTask.Task.Club?.Name ?? "Unknown club",
                SenderName = "VolleyMS", 
                SenderSurname = " ",
                TaskId = userTask.TaskId
            };

            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, null);
            if (payloadResult.IsFailure) throw new DomainException(payloadResult.Error);

            var notificationResult = Notification.Create(
                notificationRequest.RequiredClubMemberRole,
                notificationRequest.NotificationCategory,
                notificationRequest.Text,
                notificationRequest.LinkedURL,
                payloadResult.Value,
                assignee // Sender
            );

            if (notificationResult.IsFailure)
                throw new DomainException(notificationResult.Error);

            var notification = notificationResult.Value;
            var sendResult = notification.Send(new List<User> { assignee });

            if (sendResult.IsFailure)
                throw new DomainException(sendResult.Error);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
