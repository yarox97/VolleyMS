using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.RequestToJoinClub
{
    public record RequestToJoinClubCommand (Guid requestorId, string joinCode) : IRequest<Result>
    {
    }
}
