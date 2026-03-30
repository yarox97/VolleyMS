namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Tasks
{
    public sealed record TaskAssignedPayload(
        DateTime StartDateTime, 
        DateTime EndDateTime, 
        string TaskPriority, 
        string TaskTitle, 
        string Description, 
        string TaskPenaltyValue,
        string TaskPenaltyType,
        string ClubName, 
        string AssignerFullName, 
        Guid TaskId)
    {
    }
}
