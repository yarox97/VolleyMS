using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.GetAdjustments
{
    public sealed record GetPayslipAdjustmentsQuery(Guid PayslipId, Guid RequestorId, int page, int pageSize) : IRequest<Result<PaginatedResult<PayslipAdjustmentDto>>>;
}
