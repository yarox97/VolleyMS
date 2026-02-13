using System.Net.Mail;
using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class UserTaskResponse : BaseEntity
    {
        public Guid Id { get; private init; }
        public ICollection<UserTaskResponseAttachment> Attachments { get; private set; } = new List<UserTaskResponseAttachment>();
        public Task Task { get; private init; }
        public Guid TaskId { get; private init; }
        public User User { get; private set; }
        public Guid UserId { get; private set; }
        public string? Text { get; private set; }

        private UserTaskResponse()
            : base(Guid.Empty)
        {
        }

        private UserTaskResponse(Guid id, IList<UserTaskResponseAttachment> attachments, Task task, User user, string? text)
            : base(id)
        {
            Id = id;
            Attachments = attachments;
            Task = task;
            TaskId = task.Id;
            Text = text;
            User = user;
            UserId = user.Id;
        }

        public static Result<UserTaskResponse> Create(IList<UserTaskResponseAttachment>? attachments, Task task, User user, string? text)
        {
            if(attachments?.Count > 5)
            {
                return Result.Failure<UserTaskResponse>(DomainErrors.Attachment.TooManyFiles);
            }

            if (task is null) 
            {
                return Result.Failure<UserTaskResponse>(DomainErrors.Task.TaskNotFound);
            }

            if(user is null)
            {
                return Result.Failure<UserTaskResponse>(DomainErrors.User.UserNotFound);
            }

            return new UserTaskResponse(Guid.NewGuid(), attachments, task, user, text);
        }
    }
}
