using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Club.Get
{
    public sealed record GetClubQuery(Guid clubId, Guid? userId) : IRequest<Result<ClubDto>>
    {
    }
}
