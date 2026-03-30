namespace VolleyMS.Core.DomainEvents.Tasks
{
    public sealed record UserTaskCompletedDomainEvent(Guid UserTaskId) : IDomainEvent
    {
    }
}