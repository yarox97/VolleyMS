namespace VolleyMS.Core.DomainEvents
{
    public sealed record JoinClubRequestSentDomainEvent(Guid requestorId, Guid clubId, string requestorName, string requestorSurname,  string clubName) : IDomainEvent
    {
    }
}