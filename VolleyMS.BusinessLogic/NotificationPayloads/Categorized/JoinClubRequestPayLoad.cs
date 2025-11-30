namespace VolleyMS.BusinessLogic.NotificationPayloads.Categorized
{
    public record JoinClubRequestPayLoad(Guid RequestorId, Guid ClubId, string ClubName, string Name, string Surname);
}
