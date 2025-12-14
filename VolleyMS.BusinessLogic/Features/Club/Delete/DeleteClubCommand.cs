using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Club.Delete
{
    public sealed record DeleteClubCommand(Guid clubId, Guid clubPresidentId) : IRequest<Result<Guid>>
    {
    }
}
