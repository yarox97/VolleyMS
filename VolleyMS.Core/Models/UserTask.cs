using System.Text.Json.Serialization;
using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class UserTask : BaseEntity
    {
        private readonly List<UserTaskResponse> _taskResponses = new();
        private readonly List<Comment> _comments = new();

        public UserTask() : base(Guid.Empty)
        {
        }

        public IReadOnlyCollection<UserTaskResponse> TaskResponses => _taskResponses.AsReadOnly();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid TaskId { get; private set; }
        public Task Task { get; private set; }
        public TaskStatus Status { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        public static Result<UserTask> Create(User user, Task task)
        {
            var userTask = new UserTask
            {
                Id = Guid.NewGuid(),
                Task = task,
                TaskId = task.Id,
                User = user,
                UserId = user.Id,
                Status = TaskStatus.Uncompleted,
            };

            return Result.Success(userTask);
        }
        public Result Complete()
        {
            CompletedAt = DateTime.UtcNow;
            Status = TaskStatus.Completed;

            return Result.Success();
        }

        public Result AddUserResponse(IList<MediaFile>? attachments, string? text)
        {
            if (attachments.Count >= 5)
                return Result.Failure(DomainErrors.Task.MaxProofsAllowed);


            var userTaskResponseResult = UserTaskResponse.Create(this, text, attachments);

            if (userTaskResponseResult.IsFailure)
                return Result.Failure(userTaskResponseResult.Error);

            _taskResponses.Add(userTaskResponseResult.Value);
            return Result.Success();
        }


        public Result RemoveProof(Guid proofId)
        {
            var proof = _taskResponses.FirstOrDefault(p => p.Id == proofId);
            if (proof == null)
                return Result.Failure(DomainErrors.Task.ProofNotFound);

            _taskResponses.Remove(proof);
            return Result.Success();
        }
    }
}
