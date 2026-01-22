namespace VolleyMS.Core.DomainEvents.Contracts
{
    public sealed record ContractAssignedDomainEvent(Guid userId, Guid assignerId, Guid clubId) : IDomainEvent;
}
