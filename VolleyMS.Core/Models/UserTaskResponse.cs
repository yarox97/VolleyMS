using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class UserTaskResponse : BaseEntity
    {
        private readonly List<UserTaskResponseAttachment> _attachments = new();
        public UserTask UserTask { get; private set; }
        public Guid UserTaskId { get; private set; }
        public string? Text { get; private set; }

        public ICollection<UserTaskResponseAttachment>? Attachments => _attachments.AsReadOnly();
        private UserTaskResponse()
            : base(Guid.Empty)
        {
        }

        private UserTaskResponse(Guid id, UserTask userTask, string? text)
            : base(id)
        {
            Id = id;
            Text = text;
            UserTask = userTask;
            UserTaskId = userTask.Id;
        }

        public static Result<UserTaskResponse> Create(UserTask userTask, IList<UserTaskResponseAttachment>? attachments, string? text)
        {
            if(attachments?.Count > 5)
            {
                return Result.Failure<UserTaskResponse>(DomainErrors.Attachment.TooManyFiles);
            }

            if(userTask == null)
            {
                return Result.Failure<UserTaskResponse>(DomainErrors.UserTask.UserTaskNotFound);
            }

            var userTaskResponse = new UserTaskResponse(Guid.NewGuid(), userTask, text);

            if (attachments != null)
            {
                foreach (var userTaskResponseAttachment in attachments)
                {
                    if (userTaskResponse._attachments.Count >= 5)
                        return Result.Failure<UserTaskResponse>(DomainErrors.Attachment.TooManyFiles);

                    // Создаем связь
                    userTaskResponse._attachments.Add(new UserTaskResponseAttachment(userTaskResponse, userTaskResponseAttachment.Media));
                }
            }

            return Result.Success(userTaskResponse);
        }
    }
}
