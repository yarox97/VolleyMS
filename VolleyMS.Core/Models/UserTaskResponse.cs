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

        public static Result<UserTaskResponse> Create(
            UserTask userTask,
            string? text,
            IEnumerable<MediaFile>? mediaFiles)
        {
            if (userTask == null)
                return Result.Failure<UserTaskResponse>(DomainErrors.UserTask.UserTaskNotFound);

            var response = new UserTaskResponse(Guid.NewGuid(), userTask, text);

            if (mediaFiles != null && mediaFiles.Any())
            {
                if (mediaFiles.Count() > 5)
                    return Result.Failure<UserTaskResponse>(DomainErrors.Attachment.TooManyFiles);

                foreach (var media in mediaFiles)
                {
                    var attachment = new UserTaskResponseAttachment(response, media);
                    response._attachments.Add(attachment);
                }
            }

            return Result.Success(response);
        }
    }
}
