using MediatR;
using VolleyMS.BusinessLogic.Features.Contracts.Get;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Contracts.GetUsersAll
{
    public sealed record GetUsersAllContractsQuery(Guid userId, int pageNumber, int pageSize) : IRequest<Result<PaginatedResult<ContractDto>>>;
}
