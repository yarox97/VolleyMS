using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class Task : BaseEntity
    {
        private readonly List<User> _receivers = new();
        private readonly List<Comment> _comments = new();

        private Task() : base(Guid.Empty)
        {
            DayOfWeek = new List<DayOfWeek>();
        }

        private Task(
            Guid id,
            TaskType taskType,
            TaskStatus taskStatus,
            PenaltyType penaltyType,
            DateTime startDate,
            DateTime? endDate,
            List<DayOfWeek> dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            string title,
            string description,
            Guid clubId,
            Guid senderId)
            : base(id)
        {
            TaskType = taskType;
            TaskStatus = taskStatus;
            PenaltyType = penaltyType;
            StartDate = startDate;
            EndDate = endDate;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
            Title = title;
            Description = description;
            ClubId = clubId;
            SenderId = senderId;
        }

        public TaskType TaskType { get; private set; }
        public TaskStatus TaskStatus { get; private set; }
        public PenaltyType PenaltyType { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public List<DayOfWeek> DayOfWeek { get; private set; }

        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public string Title { get; private set; }
        public string Description { get; private set; }

        public Guid SenderId { get; private set; }
        public User Sender { get; private set; }

        public Guid ClubId { get; private set; }
        public Club Club { get; private set; }

        public IReadOnlyCollection<User> Receivers => _receivers;
        public IReadOnlyCollection<Comment> Comments => _comments;

        public static Result<Task> Create(
            TaskType taskType,
            PenaltyType penaltyType,
            DateTime startDate,
            DateTime? endDate,
            List<DayOfWeek> dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            string title,
            string description,
            Guid clubId,
            Guid senderId)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Task>(DomainErrors.Task.TitleEmpty);

            if (clubId == Guid.Empty || senderId == Guid.Empty)
                return Result.Failure<Task>(Error.NullValue);

            if (endDate.HasValue && startDate > endDate.Value)
                return Result.Failure<Task>(DomainErrors.Task.InvalidDates);

            if (startTime > endTime)
                return Result.Failure<Task>(DomainErrors.Task.InvalidDates);

            if (endDate.HasValue && endDate.Value < DateTime.UtcNow)
                return Result.Failure<Task>(DomainErrors.Task.InvalidDates);

            var task = new Task(
                Guid.NewGuid(),
                taskType,
                TaskStatus.Uncompleted, 
                penaltyType,
                startDate,
                endDate,
                dayOfWeek ?? new List<DayOfWeek>(),
                startTime,
                endTime,
                title,
                description,
                clubId,
                senderId
            );

            return Result.Success(task);
        }
    }
}