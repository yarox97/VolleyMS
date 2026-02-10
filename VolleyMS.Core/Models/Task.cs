using VolleyMS.Core.Common;
using VolleyMS.Core.DomainEvents.Tasks;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class Task : BaseEntity
    {
        private readonly List<UserTask> _receivers = new();
        private readonly List<Comment> _comments = new();
        private readonly List<TaskAttachment> _attachments = new();

        private Task() : base(Guid.Empty)
        {
        }
        private Task(
            Guid id,
            string title,
            string description,
            string? payload,
            Guid clubId,
            Guid senderId)
            : base(id)
        {
            TaskType = taskType;
            PenaltyType = penaltyType;
            Title = title;
            Description = description;
            Payload = payload;
            ClubId = clubId;
            SenderId = senderId;
        }

        public TaskType TaskType { get; private set; }
        public PenaltyType PenaltyType { get; private set; }
        public TaskSchedule TaskSchedule { get; private set; }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string? Payload { get; private set; } // Json payload to a specific type of task.
        public Guid SenderId { get; private set; }
        public User Sender { get; private set; }

        public Guid ClubId { get; private set; }
        public Club Club { get; private set; }

        public IReadOnlyCollection<UserTask> Receivers => _receivers;
        public IReadOnlyCollection<Comment> Comments => _comments;
        public IReadOnlyCollection<TaskAttachment> Attachments => _attachments;

        public static Result<Task> Create(
            string title,
            string description,
            TaskSchedule taskSchedule,
            TaskType taskType,
            PenaltyType penaltyType,
            DateTime startDate,
            DateTime? endDate,
            List<DayOfWeek> dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            string title,
            string description,
            string? payload,
            Guid clubId,
            Guid senderId)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Task>(DomainErrors.Task.TitleEmpty);

            if (clubId == Guid.Empty || senderId == Guid.Empty)
                return Result.Failure<Task>(Error.NullValue);

            var task = new Task(
                Guid.NewGuid(),
                title,
                description,
                taskSchedule,
                taskType,
                penaltyType,
                startDate,
                endDate,
                dayOfWeek ?? new List<DayOfWeek>(),
                startTime,
                endTime,
                title,
                description,
                payload,
                clubId,
                senderId
            );

            return Result.Success(task);
        }

        public Result AssignTask(IEnumerable<User> Receivers)
        {
            if (Receivers == null || !Receivers.Any())
                return Result.Failure(DomainErrors.Task.NoReceivers);

            foreach(User Receiver in Receivers)
            {
                var assignTaskResult = UserTask.Create(Receiver, this);

                if (assignTaskResult.IsFailure)
                    return Result.Failure(assignTaskResult.Error);

                _receivers.Add(assignTaskResult.Value); // Add receiver 
            }

            RaiseDomainEvent(new TaskAssignedDomainEvent(Receivers, ClubId));

            return Result.Success();
        }
    }
}