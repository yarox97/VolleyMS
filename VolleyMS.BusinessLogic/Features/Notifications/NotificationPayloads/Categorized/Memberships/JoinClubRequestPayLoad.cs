namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized.Memberships
{
    public record JoinClubRequestPayLoad(Guid RequestorId, Guid ClubId, string ClubName, string Name, string Surname)
    { 
    }
}
