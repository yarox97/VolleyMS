using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;

namespace VolleyMS.Core.Entities
{
    public class Task : BaseEntity
    {
        private Task(Guid id, TaskType taskType, TaskStatus taskStatus,
                                                       PenaltyType penaltyType, DateTime startDate,
                                                       DateTime? endDate, List<DayOfWeek> dayOfWeek,
                                                       TimeSpan startTime, TimeSpan endTime,
                                                       string title, string description)
        {
            Id = id;
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
        public Guid Id { get; }
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
        
        public static (Task task, string error) Create(Guid id, TaskType taskType, TaskStatus taskStatus, 
                                                       PenaltyType penaltyType, DateTime startDate,
                                                       DateTime? endDate, List<DayOfWeek> dayOfWeek,
                                                       TimeSpan startTime, TimeSpan endTime,
                                                       string title, string description)
        {
            string error = string.Empty;
            Task task = new Task(id, taskType, taskStatus, penaltyType, startDate, endDate, dayOfWeek, startTime, endTime, title, description);

            if (string.IsNullOrEmpty(title))
            {
                error = "Title cannot be empty!";
            }
            if(endDate <= task.CreatedAt || startTime > endTime || startDate > endDate)
            {
                error = "Task date is outside the bounds!";
            }

            return (task, error);
        }
    }
}
