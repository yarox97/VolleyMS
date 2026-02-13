using VolleyMS.Core.Models;

namespace VolleyMS.Core.DomainEvents.Tasks
{
    public sealed record TaskAssignedDomainEvent(IEnumerable<Guid> receivers, Guid clubId) : IDomainEvent;
}
