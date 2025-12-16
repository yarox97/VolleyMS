using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.ClubMembers.Get
{
    public sealed record GetMembersQuery(Guid clubId, Guid requestorId) : IRequest<Result<IEnumerable<ClubMemberDto>>>
    {
    }
}
