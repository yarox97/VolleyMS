using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.ApproveRequest
{
    public sealed record ApproveRequestToJoinClubCommand(
    Guid joinClubRequestId,
    Guid responserId,
    ClubMemberRole clubMemberRole = ClubMemberRole.Player
    ) : IRequest<Result>;

}
