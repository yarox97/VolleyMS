using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Club.UpdatePrimaryData
{
    public sealed record UpdatePrimaryDataCommand(Guid ClubId, Guid RequestorId, string? Description, string? Name) : IRequest<Result>;
}
