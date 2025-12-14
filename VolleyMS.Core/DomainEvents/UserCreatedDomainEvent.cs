namespace VolleyMS.Core.DomainEvents
{
    public sealed record UserCreatedDomainEvent(Guid userId) : IDomainEvent
    {
    }
}
