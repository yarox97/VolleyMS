namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized
{
    public sealed record ContractAssignedPayLoad(Guid ClubId, string ClubName, string AssignerName, string AssignerSurname)
    {
    }
}
