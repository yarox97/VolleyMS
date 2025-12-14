using VolleyMS.Core.DomainEvents;

namespace VolleyMS.Core.Models
{
    public sealed record JoinClubRequestSent(Guid userId, Guid clubId) : IDomainEvent
    {
    }
}