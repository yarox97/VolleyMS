using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Contracts.Get
{
    public sealed record GetContractQuery(Guid contractId, Guid requestorId) : IRequest<Result<ContractDto>>
    {
    }
}
