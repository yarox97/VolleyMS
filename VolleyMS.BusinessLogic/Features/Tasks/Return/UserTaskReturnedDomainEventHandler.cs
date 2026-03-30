using MediatR;
using VolleyMS.Core.DomainEvents.Tasks;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Exceptions;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Services;

namespace VolleyMS.BusinessLogic.Features.Tasks.Return
{
    public class UserTaskReturnedDomainEventHandler : INotificationHandler<UserTaskReturnedDomainEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public UserTaskReturnedDomainEventHandler(
            INotificationService notificationService,
            INotificationRepository notificationRepository,
            IUserTaskRepository userTaskRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _userTaskRepository = userTaskRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async System.Threading.Tasks.Task Handle(UserTaskReturnedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // get user task details
            var userTask = await _userTaskRepository.GetByIdWithDetailsAsync(domainEvent.UserTaskId, cancellationToken);

            if (userTask is null)
                throw new DomainException(DomainErrors.Task.TaskNotFound);

            var assignee = userTask.User;
            var assigner = userTask.Task.Sender;

            // create notification request
            var notificationRequest = new NotificationRequest
            {
                NotificationCategory = NotificationCategory.TaskReturned,
                Text = $"Your task '{userTask.Task.Title}' has been returned for revision.",
                LinkedURL = $"/tasks/{userTask.TaskId}",

                StartsFrom = userTask.Task.TaskSchedule.StartDate,
                EndsBy = userTask.Task.TaskSchedule.EndDate,
                TaskPriority = userTask.Task.TaskPriority.ToString(),
                TaskTitle = userTask.Task.Title,
                TaskDescription = userTask.Task.Description,

                TaskPenaltyType = userTask.Task.PenaltyType.ToString(),
                TaskPenaltyValue = userTask.Task.PenaltyAmount?.ToString() ?? "0",

                ClubName = userTask.Task.Club?.Name ?? "Unknown Club",
                SenderName = assigner.Name,
                SenderSurname = assigner.Surname,
                TaskId = userTask.TaskId
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

            // send notification to assignee
            var sendResult = notification.Send(new List<User> { assignee });
            if (sendResult.IsFailure) throw new DomainException(sendResult.Error);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send Email
            string subject = "Task Returned for Revision";
            string body = $"<p>Hello {assignee.Name},</p>" +
                          $"<p>Your submission for the task '<b>{userTask.Task.Title}</b>' has been returned for revision by <b>{assigner.Name} {assigner.Surname}</b>.</p>" +
                          $"<p>Please review the feedback and submit your changes.</p>" +
                          $"<p><a href='https://yourvolleymsapp.com/tasks/{userTask.TaskId}'>Click here to view the task details</a></p>" +
                          "<br><p>Best regards,<br/>VolleyMS Team</p>";

            await _emailService.SendEmailAsync(assignee.Email, subject, body);
        }
    }
}
