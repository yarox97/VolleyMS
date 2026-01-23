using MediatR;
using VolleyMS.BusinessLogic.Features.Contracts.Get;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Contracts.GetClubsAll
{
    public sealed record GetClubsAllContractsQuery(Guid clubId, int pageSize, int pageNumber, Guid requestorId) : IRequest<Result<PaginatedResult<ContractDto>>>;
}
