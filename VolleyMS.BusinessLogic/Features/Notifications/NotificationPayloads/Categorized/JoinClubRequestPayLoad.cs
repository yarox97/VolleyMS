namespace VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads.Categorized
{
    public record JoinClubRequestPayLoad(Guid RequestorId, Guid ClubId, string ClubName, string Name, string Surname)
    { 
    }
}
