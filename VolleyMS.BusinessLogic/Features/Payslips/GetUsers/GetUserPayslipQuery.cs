using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.GetUsers
{
    public record GetUserPayslipsQuery(Guid UserId, int page, int pageSize, Guid? ContactId = null) : IRequest<Result<PaginatedResult<PayslipDto>>>;
}
