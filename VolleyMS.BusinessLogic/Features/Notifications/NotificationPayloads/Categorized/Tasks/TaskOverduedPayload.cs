namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Tasks
{
    public sealed record TaskOverduedPayload(
        DateTime StartDateTime,
        DateTime EndDateTime,
        string TaskPriority,
        string TaskTitle,
        string Description,
        string ClubName,
        string AssignerFullName,
        Guid TaskId)
    {
    }
}
