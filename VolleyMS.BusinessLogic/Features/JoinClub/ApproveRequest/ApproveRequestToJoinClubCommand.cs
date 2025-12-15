using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.ApproveRequest
{
    public sealed record ApproveRequestToJoinClubCommand(Guid joinClubRequestId, ClubMemberRole clubMemberRole, Guid responserId) : IRequest<Result>
    {
    }
}
