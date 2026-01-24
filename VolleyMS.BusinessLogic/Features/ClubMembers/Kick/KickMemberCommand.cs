using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.ClubMembers.Kick
{
    public sealed record KickMemberCommand(Guid clubId, Guid memberId, Guid responserId) : IRequest<Result>;
}
