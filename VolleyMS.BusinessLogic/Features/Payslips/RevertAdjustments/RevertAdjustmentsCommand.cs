using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.RevertAdjustments
{
    public record RevertAdjustmentsCommand(IEnumerable<Guid> taskAdjustmentIds, Guid userId) : IRequest<Result>
    {
    }
}
