using VolleyMS.Core.DomainEvents;

namespace VolleyMS.Core.DomainEvents.Tasks
{
    public sealed record UserTaskOverdueDomainEvent(Guid UserTaskId) : IDomainEvent
    {
    }
}