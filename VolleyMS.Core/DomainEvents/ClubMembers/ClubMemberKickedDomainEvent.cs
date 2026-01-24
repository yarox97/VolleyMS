using MediatR;

namespace VolleyMS.Core.DomainEvents.ClubMembers
{
    public sealed record ClubMemberKickedDomainEvent(Guid userId, Guid clubId) : IDomainEvent;
}
