using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;
using VolleyMS.Core.Exceptions;

namespace VolleyMS.Core.Entities
{
    public class Task : BaseEntity
    {
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
        public TaskType TaskType { get; }
        public TaskStatus TaskStatus { get; }
        public PenaltyType PenaltyType { get; } = PenaltyType.None;
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }
        public List<DayOfWeek> DayOfWeek { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }

        public string Title = string.Empty;
        public string Description = string.Empty;  
        
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
