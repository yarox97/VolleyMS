namespace VolleyMS.Core.DomainEvents
{
    public sealed record JoinClubRequestSentDomainEvent(Guid requestorId, Guid clubId) : IDomainEvent
    {
    }
}