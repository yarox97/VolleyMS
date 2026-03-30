namespace VolleyMS.Core.DomainEvents.Tasks
{
    public record TaskUpdatedDomainEvent(
        Guid TaskId,
        string OldTitle,
        string OldDescription,
        TaskPriority OldTaskPriority,
        DateTime OldStartDate,
        DateTime OldEndDate
    ) : IDomainEvent;
}
