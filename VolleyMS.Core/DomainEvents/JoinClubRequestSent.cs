namespace VolleyMS.Core.DomainEvents
{
    public sealed record JoinClubRequestSent(Guid userId, Guid clubId) : IDomainEvent
    {
    }
}