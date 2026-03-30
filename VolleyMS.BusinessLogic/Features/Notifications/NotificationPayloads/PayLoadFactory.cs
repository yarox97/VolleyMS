using System.Text.Json;
using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Memberships;
using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Contracts;
using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Tasks;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Shared;
using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized;

namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads
{
    public static class PayLoadFactory
    {
        public static Result<string>? CreatePayLoad(NotificationRequest notificationRequest, Guid? senderId)
        {
            return notificationRequest.NotificationCategory switch
            {
                NotificationCategory.ClubJoinRequest => JsonSerializer.Serialize(new JoinClubRequestPayLoad(
                    (Guid)senderId,
                    notificationRequest.ClubId ?? throw new ArgumentException("ClubId is required"),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    notificationRequest.SenderName ?? throw new ArgumentException("SenderName is required"),
                    notificationRequest.SenderSurname ?? throw new ArgumentException("SenderSurname is required")
                )),

                NotificationCategory.ClubJoinApproved => JsonSerializer.Serialize(new JoinClubRequestApprovedPayload(
                    notificationRequest.ClubId ?? throw new ArgumentException("ClubId is required"),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required")
                )),

                NotificationCategory.ClubJoinRejected => JsonSerializer.Serialize(new JoinClubRequestRejectedPayLoad(
                    notificationRequest.ClubId ?? throw new ArgumentException("ClubId is required"),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required")
                )),

                NotificationCategory.ContractAssigned => JsonSerializer.Serialize(new ContractAssignedPayLoad(
                    notificationRequest.ClubId ?? throw new ArgumentException("ClubId is required"),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    notificationRequest.SenderName ?? throw new ArgumentException("SenderName is required"),
                    notificationRequest.SenderSurname ?? throw new ArgumentException("SenderSurname is required")
                )),

                NotificationCategory.KickedFromClubByClub => JsonSerializer.Serialize(new ClubMemberKickedPayLoad(
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    notificationRequest.ClubId ?? throw new ArgumentException("ClubId is required")
                )),

                NotificationCategory.TaskAssigned => JsonSerializer.Serialize(new TaskAssignedPayload(
                    notificationRequest.StartsFrom ?? throw new ArgumentException("EventStartTime is required"),
                    notificationRequest.EndsBy ?? throw new ArgumentException("TaskDueDate is required"),
                    notificationRequest.TaskPriority ?? throw new ArgumentException("TaskPriority is required"),
                    notificationRequest.TaskTitle ?? throw new ArgumentException("TaskTitle is required"),
                    notificationRequest.TaskDescription ?? throw new ArgumentException("TaskDescription is required"),
                    notificationRequest.TaskPenaltyValue ?? "0",
                    notificationRequest.TaskPenaltyType ?? PenaltyType.None.ToString(),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    $"{notificationRequest.SenderName} {notificationRequest.SenderSurname}".Trim() ?? throw new ArgumentException("Assigner credentials are required"),
                    notificationRequest.TaskId ?? throw new ArgumentException("TaskId is required")
                )),

                NotificationCategory.TaskModified => JsonSerializer.Serialize(new TaskModifiedPayload(
                    notificationRequest.OldStartsFrom ?? throw new ArgumentException("OldEventStartTime is required"),
                    notificationRequest.OldEndsBy ?? throw new ArgumentException("OldTaskDueDate is required"),
                    notificationRequest.StartsFrom ?? throw new ArgumentException("NewEventStartTime is required"),
                    notificationRequest.EndsBy ?? throw new ArgumentException("NewTaskDueDate is required"),
                    notificationRequest.OldTaskPriority ?? throw new ArgumentException("OldTaskPriority is required"),
                    notificationRequest.TaskPriority ?? throw new ArgumentException("NewTaskPriority is required"),
                    notificationRequest.OldTaskTitle ?? throw new ArgumentException("OldTaskTitle is required"),
                    notificationRequest.TaskTitle ?? throw new ArgumentException("NewTaskTitle is required"),
                    notificationRequest.OldTaskDescription ?? throw new ArgumentException("OldTaskDescription is required"),
                    notificationRequest.TaskDescription ?? throw new ArgumentException("NewTaskDescription is required"),
                    notificationRequest.OldTaskPenaltyValue ?? "0",
                    notificationRequest.TaskPenaltyValue ?? "0",
                    notificationRequest.OldTaskPenaltyType ?? PenaltyType.None.ToString(),
                    notificationRequest.TaskPenaltyType ?? PenaltyType.None.ToString(),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    $"{notificationRequest.SenderName} {notificationRequest.SenderSurname}".Trim() ?? throw new ArgumentException("Assigner credentials are required"),
                    notificationRequest.TaskId ?? throw new ArgumentException("TaskId is required")
                )),

                NotificationCategory.TaskOverdue => JsonSerializer.Serialize(new TaskOverduedPayload(
                    notificationRequest.StartsFrom ?? throw new ArgumentException("EventStartTime is required"),
                    notificationRequest.EndsBy ?? throw new ArgumentException("TaskDueDate is required"),
                    notificationRequest.TaskPriority ?? throw new ArgumentException("TaskPriority is required"),
                    notificationRequest.TaskTitle ?? throw new ArgumentException("TaskTitle is required"),
                    notificationRequest.TaskDescription ?? throw new ArgumentException("TaskDescription is required"),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    $"{notificationRequest.SenderName} {notificationRequest.SenderSurname}".Trim() ?? throw new ArgumentException("Assigner credentials are required"),
                    notificationRequest.TaskId ?? throw new ArgumentException("TaskId is required")
                )),

                NotificationCategory.TaskCompleted => JsonSerializer.Serialize(new TaskCompletedPayload(
                    notificationRequest.StartsFrom ?? throw new ArgumentException("EventStartTime is required"),
                    notificationRequest.EndsBy ?? throw new ArgumentException("TaskDueDate is required"),
                    notificationRequest.CompletedAt ?? throw new ArgumentException("CompletedAt is required"),
                    notificationRequest.TaskPriority ?? throw new ArgumentException("TaskPriority is required"),
                    notificationRequest.TaskTitle ?? throw new ArgumentException("TaskTitle is required"),
                    notificationRequest.TaskDescription ?? throw new ArgumentException("TaskDescription is required"),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    $"{notificationRequest.SenderName} {notificationRequest.SenderSurname}".Trim() ?? throw new ArgumentException("Assigner credentials are required"),
                    notificationRequest.TaskId ?? throw new ArgumentException("TaskId is required")
                )),

                NotificationCategory.TaskConfirmed => JsonSerializer.Serialize(new TaskConfirmedPayload(
                    notificationRequest.StartsFrom ?? throw new ArgumentException("EventStartTime is required"),
                    notificationRequest.EndsBy ?? throw new ArgumentException("TaskDueDate is required"),
                    notificationRequest.ConfirmedAt ?? throw new ArgumentException("ConfirmedAt is required"),
                    notificationRequest.TaskPriority ?? throw new ArgumentException("TaskPriority is required"),
                    notificationRequest.TaskTitle ?? throw new ArgumentException("TaskTitle is required"),
                    notificationRequest.TaskDescription ?? throw new ArgumentException("TaskDescription is required"),
                    notificationRequest.TaskPenaltyValue ?? "0",
                    notificationRequest.TaskPenaltyType ?? PenaltyType.None.ToString(),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    $"{notificationRequest.SenderName} {notificationRequest.SenderSurname}".Trim() ?? throw new ArgumentException("Assigner credentials are required"),
                    notificationRequest.TaskId ?? throw new ArgumentException("TaskId is required")
                )),

                NotificationCategory.TaskReturned => JsonSerializer.Serialize(new TaskAssignedPayload(
                    notificationRequest.StartsFrom ?? throw new ArgumentException("EventStartTime is required"),
                    notificationRequest.EndsBy ?? throw new ArgumentException("TaskDueDate is required"),
                    notificationRequest.TaskPriority ?? throw new ArgumentException("TaskPriority is required"),
                    notificationRequest.TaskTitle ?? throw new ArgumentException("TaskTitle is required"),
                    notificationRequest.TaskDescription ?? throw new ArgumentException("TaskDescription is required"),
                    notificationRequest.TaskPenaltyValue ?? "0",
                    notificationRequest.TaskPenaltyType ?? PenaltyType.None.ToString(),
                    notificationRequest.ClubName ?? throw new ArgumentException("ClubName is required"),
                    $"{notificationRequest.SenderName} {notificationRequest.SenderSurname}".Trim() ?? throw new ArgumentException("Assigner credentials are required"),
                    notificationRequest.TaskId ?? throw new ArgumentException("TaskId is required")
                )),
                // Можно добавить другие категории здесь
                _ => null
            };
        }
    }
}
