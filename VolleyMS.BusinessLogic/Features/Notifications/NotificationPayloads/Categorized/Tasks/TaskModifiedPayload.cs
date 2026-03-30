namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Tasks
{
    public sealed record TaskModifiedPayload(
        DateTime OldStartDateTime, 
        DateTime OldEndDateTime, 

        DateTime NewStartDateTime, 
        DateTime NewEndDateTime,

        string OldTaskPriority,
        string NewTaskPriority,

        string OldTaskTitle,
        string NewTaskTitle,

        string OldDescription,
        string NewDescription,

        string OldTaskPenaltyValue,
        string NewTaskPenaltyValue,

        string OldTaskPenaltyType,
        string NewTaskPenaltyType,

        string ClubName,
        string AssignerFullName,

        Guid TaskId)
    {
    }
}
