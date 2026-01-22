namespace VolleyMS.Core.DomainEvents.ClubMembers
{
    public sealed record JoinClubApprovedDomainEvent(Guid userId, Guid clubId, Guid responserId, string clubName) : IDomainEvent;

}
