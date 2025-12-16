namespace VolleyMS.Core.DomainEvents
{
    public sealed record JoinClubApprovedDomainEvent(Guid userId, Guid clubId, Guid responserId, string clubName) : IDomainEvent
    {
    }
}
