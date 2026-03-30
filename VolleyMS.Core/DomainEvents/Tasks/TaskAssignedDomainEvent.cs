using VolleyMS.Core.Models;

namespace VolleyMS.Core.DomainEvents.Tasks
{
    public record TaskAssignedDomainEvent(
        Guid TaskId,
        IEnumerable<User> Receivers,
        Guid ClubId
    ) : IDomainEvent;
}
