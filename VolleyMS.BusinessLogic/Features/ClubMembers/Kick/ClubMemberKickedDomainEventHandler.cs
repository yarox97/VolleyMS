using MediatR;
using VolleyMS.Core.DomainEvents.ClubMembers;

namespace VolleyMS.BusinessLogic.Features.ClubMembers.Kick
{
    public sealed class ClubMemberKickedDomainEventHandler : INotificationHandler<ClubMemberKickedDomainEvent>
    {
        public Task Handle(ClubMemberKickedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
