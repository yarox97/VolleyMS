namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Tasks
{
    public sealed record TaskCompletedPayload(
        DateTime StartDateTime,
        DateTime EndDateTime,
        DateTime CompletedAt,
        string TaskPriority,
        string TaskTitle,
        string Description,
        string ClubName,
        string MemberFullName,
        Guid TaskId)
    {
    }
}
