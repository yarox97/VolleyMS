namespace VolleyMS.Core.DomainEvents
{
    public sealed record JoinClubApprovedDomainEvent(Guid userId, Guid clubId) : IDomainEvent
    {
    }
}
