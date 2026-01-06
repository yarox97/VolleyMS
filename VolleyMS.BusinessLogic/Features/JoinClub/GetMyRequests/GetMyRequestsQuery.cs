using MediatR;
using VolleyMS.BusinessLogic.Features.JoinClub.GetRequests;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.GetMyRequests
{
    public sealed record GetMyRequestsQuery(Guid userId) : IRequest<Result<List<JoinClubRequestDto>>>;
}
