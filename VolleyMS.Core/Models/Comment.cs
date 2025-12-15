using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class Comment : BaseEntity
    {
        private Comment() : base(Guid.Empty)
        {
        }
        private Comment(Guid id, string text, Guid taskId, Guid senderId)
            : base(id)
        {
            Text = text;
            TaskId = taskId;
            SenderId = senderId;
        }
        public string Text { get; private set; }
        public Guid TaskId { get; private set; }
        public Task Task { get; private set; }
        public Guid SenderId { get; private set; }
        public User Sender { get; private set; }

        public static Result<Comment> Create(string text, Guid taskId, Guid senderId)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Result.Failure<Comment>(DomainErrors.Comment.TextEmpty);

            if (taskId == Guid.Empty || senderId == Guid.Empty)
                return Result.Failure<Comment>(Error.NullValue);

            var comment = new Comment(Guid.NewGuid(), text, taskId, senderId);

            return Result.Success(comment);
        }

        public Result Update(string newText, Guid editorId)
        {
            if (editorId != SenderId)
                return Result.Failure(DomainErrors.Comment.NotAuthor);

            if (string.IsNullOrWhiteSpace(newText))
                return Result.Failure(DomainErrors.Comment.TextEmpty);

            Text = newText;
            UpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }
    }
}