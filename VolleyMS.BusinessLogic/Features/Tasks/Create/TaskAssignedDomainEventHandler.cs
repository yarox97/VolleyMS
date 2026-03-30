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

namespace VolleyMS.BusinessLogic.Features.Tasks.Create
{
    public class TaskAssignedDomainEventHandler : INotificationHandler<TaskAssignedDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public TaskAssignedDomainEventHandler(
            INotificationService notificationService,
            INotificationRepository notificationRepository,
            ITaskRepository taskRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _taskRepository = taskRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async System.Threading.Tasks.Task Handle(TaskAssignedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // get task details
            var task = await _taskRepository.GetByIdWithDetailsAsync(domainEvent.TaskId, cancellationToken);

            if (task is null)
                throw new DomainException(DomainErrors.Task.TaskNotFound);

            var assigner = task.Sender;
            var receiverIds = domainEvent.Receivers.Select(r => r.Id).ToList();

            var receivers = task.Receivers
            .Select(r => r.User)
            .Where(u => receiverIds.Contains(u.Id))
            .ToList();

            if (receiverIds.Count != receivers.Count) return;

            // create notification request
            var notificationRequest = new NotificationRequest
            {
                NotificationCategory = NotificationCategory.TaskAssigned,
                Text = $"New task assigned '{task.Title}'.",
                LinkedURL = $"/tasks/{task.Id}",

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
                null,
                notificationRequest.NotificationCategory,
                notificationRequest.Text,
                notificationRequest.LinkedURL,
                payloadResult.Value,
                assigner);

            if (notificationResult.IsFailure) throw new DomainException(notificationResult.Error);

            var notification = notificationResult.Value;

            // send notification to receivers
            var sendResult = notification.Send(receivers);
            if (sendResult.IsFailure) throw new DomainException(sendResult.Error);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // send email notifications
            string subject = "You were assigned to a new Task!";
            foreach (var user in receivers)
            {
                string body = $"<p>Hello, {user.Name}!</p>" +
                              $"<p>User <b>{assigner.Name} {assigner.Surname}</b> has assigned you to a task '<b>{task.Title}</b>' in club {task.Club.Name}.</p>" +
                              $"<p>Deadline: {task.TaskSchedule.EndDate:dd.MM.yyyy HH:mm}.</p>" +
                              $"<p><a href='https://yourvolleymsapp.com/tasks/{task.Id}'>Go to task</a></p>";

                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
        }
    }
}
