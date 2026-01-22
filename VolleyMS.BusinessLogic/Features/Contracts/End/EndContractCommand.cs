using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Contracts.End
{
    public sealed record EndContractCommand(Guid userId, Guid contractId) : IRequest<Result>;
}
