namespace VolleyMS.Core.DomainEvents.Contracts
{
    public sealed record ContractEndedByAssignerDomainEvent(Guid contractId, Guid assignerId, Guid userId, Guid clubId) : IDomainEvent
    {
    }
}
