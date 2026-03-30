namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Tasks
{
    public sealed record TaskConfirmedPayload(
        DateTime StartDateTime,
        DateTime EndDateTime,
        DateTime ConfirmedAt,
        string TaskPriority,
        string TaskTitle,
        string Description,
        string TaskPenaltyValue,
        string TaskPenaltyType,
        string ClubName,
        string ConfirmerFullName,
        Guid TaskId)
    {
    }
}
