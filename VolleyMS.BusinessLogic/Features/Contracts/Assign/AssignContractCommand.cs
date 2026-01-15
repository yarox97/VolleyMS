using MediatR;
using VolleyMS.Core.Models;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Contracts.Assign
{
    public sealed record AssignContractCommand(
        decimal? monthlySalary, 
        Currency? currency, 
        DateTime beginsFrom, 
        DateTime endsBy, 
        Guid memberId, 
        Guid clubId, 
        Guid responserId) : IRequest<Result>;
}
