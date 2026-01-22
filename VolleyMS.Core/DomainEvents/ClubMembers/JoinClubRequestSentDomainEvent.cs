namespace VolleyMS.Core.DomainEvents.ClubMembers
{
    public sealed record JoinClubRequestSentDomainEvent(Guid requestorId, Guid clubId, string requestorName, string requestorSurname,  string clubName) : IDomainEvent
    {
    }
}