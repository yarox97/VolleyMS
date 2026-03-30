namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Contracts
{
    public sealed record ContractAssignedPayLoad(Guid ClubId, string ClubName, string AssignerName, string AssignerSurname)
    {
    }
}
