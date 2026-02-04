using VolleyMS.Core.Models;

namespace VolleyMS.Core.DomainEvents.Tasks
{
    public sealed record TaskAssignedDomainEvent(IEnumerable<User> receivers, Guid clubId) : IDomainEvent;
}
