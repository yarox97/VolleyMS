using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.GetRequests
{
    public sealed record GetAllRequestsQuery(Guid clubId, Guid userId) : IRequest<Result<List<JoinClubRequestDto>>>
    {
    }
}
