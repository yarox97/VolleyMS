using MediatR;
using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized;
using VolleyMS.Core.DomainEvents.Tasks;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Exceptions;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;
using Task = System.Threading.Tasks.Task;

namespace VolleyMS.BusinessLogic.Features.Tasks.Complete
{
    public class UserTaskCompletedDomainEventHandler : INotificationHandler<UserTaskCompletedDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public UserTaskCompletedDomainEventHandler(
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

        public async Task Handle(UserTaskCompletedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // get user task with details
            var userTask = await _userTaskRepository.GetByIdWithDetailsAsync(domainEvent.UserTaskId, cancellationToken);

            if (userTask is null)
                throw new DomainException(DomainErrors.Task.TaskNotFound);

            var assignee = userTask.User; // person who completed the task

            var notificationRecipient = userTask.Task.Sender;

            // create notification request
            var notificationRequest = new NotificationRequest
            {
                RequiredClubMemberRole = null,
                NotificationCategory = NotificationCategory.TaskCompleted,
                Text = $"User {assignee.Name +" "+ assignee.Surname} has finished task '{userTask.Task.Title}' that you assigned",
                LinkedURL = $"/tasks/{userTask.TaskId}",

                // payload data
                StartsFrom = userTask.Task.TaskSchedule.StartDate.Date + userTask.Task.TaskSchedule.StartTime,
                EndsBy = userTask.Task.TaskSchedule.EndDate + userTask.Task.TaskSchedule.EndTime,
                CompletedAt = userTask.CompletedAt,
                TaskPriority = userTask.Task.TaskPriority.ToString(),
                TaskTitle = userTask.Task.Title,
                TaskDescription = userTask.Task.Description,
                ClubName = userTask.Task.Club?.Name ?? "Unknown club",
                SenderName = assignee.Name,
                SenderSurname = assignee.Surname,
                TaskId = userTask.TaskId
            };

            // create payload
            var payloadResult = _notificationService.HandleCategorizedPayload(notificationRequest, assignee.Id);

            if (payloadResult.IsFailure)
                throw new DomainException(payloadResult.Error);

            // create notification
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

            // on site notification sending
            var notification = notificationResult.Value;
            var sendResult = notification.Send(new List<User> { notificationRecipient });

            if (sendResult.IsFailure)
                throw new DomainException(sendResult.Error);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // email sending
            string subject = $"Task completed in {notificationRequest.ClubName}";
            string body = $"<p>User <b>{assignee.Name} {assignee.Surname}</b> has completed his task '<b>{userTask.Task.Title}</b>'.</p>" +
                          $"<p>Check it out and respond to it" +
                          $"<p><a href='https://yourvolleymsapp.com/tasks/{userTask.TaskId}'>Check task</a></p>";

            await _emailService.SendEmailAsync(notificationRecipient.Email, subject, body);
        }
    }
}