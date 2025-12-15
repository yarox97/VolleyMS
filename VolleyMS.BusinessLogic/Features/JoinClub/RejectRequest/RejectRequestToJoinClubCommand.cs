using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.RejectRequest
{
    public sealed record RejectRequestToJoinClubCommand(Guid requestId, Guid responserId) : IRequest<Result>
    {
    }
}
