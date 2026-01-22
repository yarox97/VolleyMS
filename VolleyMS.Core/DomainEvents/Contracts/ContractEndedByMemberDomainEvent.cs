namespace VolleyMS.Core.DomainEvents.Contracts
{
    public sealed record ContractEndedByMemberDomainEvent(Guid contractId, Guid userId, Guid clubId) : IDomainEvent
    {
    }
}
