namespace VolleyMS.Core.DomainEvents.Tasks
{
    public sealed record UserTaskConfirmedDomainEvent(Guid UserTaskId, TaskStatus taskStatus, Guid ConfirmatorId) : IDomainEvent
    {
    }
}