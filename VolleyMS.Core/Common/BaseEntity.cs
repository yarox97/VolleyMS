using Microsoft.EntityFrameworkCore.Query.Internal;
using VolleyMS.Core.DomainEvents;

namespace VolleyMS.Core.Common
{
    public abstract class BaseEntity : AuditableFields
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        protected BaseEntity(Guid id)
        {
            Id = id; 
        }
        public Guid Id { get; init; }
        public Guid? CreatorId { get; set; }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
