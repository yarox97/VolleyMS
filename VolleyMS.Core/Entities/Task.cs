using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;

namespace VolleyMS.Core.Entities
{
    public class Task : AuditableFields
    {
        private Task(Guid id, TaskType taskType, TaskStatus taskStatus, PenaltyType penaltyType, DateTime dueDate, string title, string description) 
        {
            Id = id;
            TaskType = taskType;
            TaskStatus = taskStatus;
            TaskStatus = taskStatus;
            PenaltyType = penaltyType;
            DueDate = dueDate;
            Title = title;
            Description = description;
        }
        public Guid Id { get; }
        public TaskType TaskType { get; }
        public TaskStatus TaskStatus { get; }
        public PenaltyType PenaltyType { get; }
        public DateTime DueDate { get; }
        public string Title = string.Empty;
        public string Description = string.Empty;  
        
        public static (Task task, string error) Create(Guid id, TaskType taskType, TaskStatus taskStatus, PenaltyType penaltyType, DateTime dueDate, string title, string description)
        {
            string error = string.Empty;
            Task task = new Task(id, taskType, taskStatus, penaltyType, dueDate, title, description);

            if (string.IsNullOrEmpty(title))
            {
                error = "Title cannot be empty!";
            }
            if(dueDate <= task.CreatedAt)
            {
                error = "Task date is outside the bounds!";
            }

            return (task, error);
        }
    }
}
