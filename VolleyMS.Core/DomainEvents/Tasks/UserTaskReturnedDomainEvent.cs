namespace VolleyMS.Core.DomainEvents.Tasks
{
    public sealed record UserTaskReturnedDomainEvent(Guid UserTaskId) : IDomainEvent
    {
    }
}