using System.Text.Json.Serialization;
using VolleyMS.Core.Common;
using VolleyMS.Core.DomainEvents.Tasks;
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
        public bool IsOverdue => (Status != TaskStatus.Confirmed || Status != TaskStatus.Completed) && DateTime.UtcNow > Task.TaskSchedule.EndDate;
        public bool IsConfirmed { get; private set; }
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
            if(IsConfirmed)
                return Result.Failure(new Error("UserTask.AlreadyConfirmed", "This task is already confirmed."));

            CompletedAt = DateTime.UtcNow;
            Status = TaskStatus.Completed;

            RaiseDomainEvent(new UserTaskCompletedDomainEvent(this.Id));

            return Result.Success();
        }

        public Result Return()
        {
            CompletedAt = null;
            Status = TaskStatus.Returned;

            RaiseDomainEvent(new UserTaskReturnedDomainEvent(this.Id));

            return Result.Success();
        }

        public Result Overdue()
        {
            if (IsOverdue)
            {
                Status = TaskStatus.Overdued;

                RaiseDomainEvent(new UserTaskOverdueDomainEvent(this.Id));
            }
            return Result.Success();
        }

        public Result Confirm(TaskStatus finalStatus, bool applyPenalty, Guid confirmatorId)
        {
            if (IsConfirmed)
                return Result.Failure(new Error("UserTask.AlreadyConfirmed", "This task is already confirmed."));

            Status = finalStatus;
            IsConfirmed = true;

            if (applyPenalty && Task.PenaltyAmount.HasValue && Task.PenaltyAmount.Value > 0)
            {
                string reason = $"Penalty / bonus for task: {Task.Title}";

                RaiseDomainEvent(new UserTaskEvaluatedDomainEvent(
                    UserId: this.UserId,
                    TaskId: this.TaskId,
                    ApplyPenalty: true,
                    PenaltyAmount: Task.PenaltyAmount.Value,
                    PenaltyType: Task.PenaltyType,
                    Reason: reason
                ));
            }

            RaiseDomainEvent(new UserTaskConfirmedDomainEvent(this.Id, finalStatus, confirmatorId));

            return Result.Success();
        }

        public Result AddUserResponse(IList<MediaFile>? attachments, string? text)
        {
            if(attachments != null)
            {
                if (attachments.Count >= 5)
                    return Result.Failure(DomainErrors.Task.MaxProofsAllowed);
            }

            var userTaskResponseResult = UserTaskResponse.Create(this, text, attachments);

            if (userTaskResponseResult.IsFailure)
                return Result.Failure(userTaskResponseResult.Error);

            _taskResponses.Add(userTaskResponseResult.Value);
            return Result.Success();
        }


    }
}
