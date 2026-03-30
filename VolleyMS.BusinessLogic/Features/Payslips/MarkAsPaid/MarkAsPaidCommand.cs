using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.MarkAsPaid
{
    public record MarkAsPaidCommand(Guid payslipId, Guid userId) : IRequest<Result>
    {
    }
}
