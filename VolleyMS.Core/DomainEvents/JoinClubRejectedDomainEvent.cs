namespace VolleyMS.Core.DomainEvents
{
    public sealed record JoinClubRejectedDomainEvent(Guid userId, Guid clubId, Guid responserId) : IDomainEvent
    {
    }
}
