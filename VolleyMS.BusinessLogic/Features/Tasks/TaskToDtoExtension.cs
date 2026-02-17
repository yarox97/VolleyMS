using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos;

namespace VolleyMS.BusinessLogic.Features.Tasks
{ 
    public static class TaskToDtoExtension
    {
        public static TaskDto ToDto(this VolleyMS.Core.Models.Task task)
        {
            return new TaskDto(
                task.Id,
                task.Title,
                task.Description,
                task.TaskType.ToString(),
                task.TaskPriority.ToString(),
                new TaskScheduleDto(
                    task.TaskSchedule.StartDate,
                    task.TaskSchedule.EndDate,
                    task.TaskSchedule.StartTime,
                    task.TaskSchedule.EndTime,
                    task.TaskSchedule.ToSystemDayOfWeekList()
                ),
                task.ClubId,
                task.Club?.Name ?? "Unknown",
                task.SenderId,
                $"{task.Sender?.Name} {task.Sender?.Surname}",
                task.Payload,
                task.Attachments.Select(a => new AttachmentDto(a.MediaFile.FileName, a.MediaFile.Url, a.MediaFile.Size)).ToList()
            );
        }
    }
}
