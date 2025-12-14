using VolleyMS.Core.Common;
using VolleyMS.Core.Exceptions;

namespace VolleyMS.Core.Models
{
    public class Task : BaseEntity
    {
        private Task() : base(Guid.Empty) 
        { 
        }
        private Task(Guid id, 
            TaskType taskType, 
            TaskStatus taskStatus,
            PenaltyType penaltyType, 
            DateTime startDate,
            DateTime? endDate, 
            List<DayOfWeek> dayOfWeek,
            TimeSpan startTime, 
            TimeSpan endTime,
            string title, 
            string description)
            : base(id)
        {
            TaskType = taskType;
            TaskStatus = taskStatus;
            TaskStatus = taskStatus;
            PenaltyType = penaltyType;
            StartDate = startDate;
            EndDate = endDate;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
            Title = title;
            Description = description;
        }
        public TaskType TaskType { get; private set; }
        public TaskStatus TaskStatus { get; private set; }
        public PenaltyType PenaltyType { get; private set; } = PenaltyType.None;
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public List<DayOfWeek> DayOfWeek { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public string Title { get; private set; } 
        public string Description { get; private set; }
        public IList<Comment> Comments { get; set; }
        public Guid SenderId { get; set; }
        public User Sender { get; set; }
        public IList<User> Receivers { get; set; }
        public Guid ClubId { get; set; }
        public Club Club { get; set; }

        public static Task Create(Guid id, 
            TaskType taskType, 
            TaskStatus taskStatus, 
            PenaltyType penaltyType, 
            DateTime startDate,
            DateTime? endDate, 
            List<DayOfWeek> dayOfWeek,
            TimeSpan startTime, 
            TimeSpan endTime,
            string title, 
            string description)
        {
            Task task = new Task(id, taskType, taskStatus, penaltyType, startDate, endDate, dayOfWeek, startTime, endTime, title, description);

            if (string.IsNullOrEmpty(title))
            {
                throw new EmptyFieldDomainException("Task title cannot be empty!");
            }
            if(endDate <= task.CreatedAt || startTime > endTime || startDate > endDate)
            {
                throw new DateOutOfBoundsDomainException("End date cannot be earlier than this moment!");
            }

            return task;
        }
    }
}
