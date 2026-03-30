using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.GetClubs
{
    public record GetClubsPayslipsQuery(
            Guid ClubId,
            Guid RequestorId,
            int Page,
            int PageSize
        ) : IRequest<Result<PaginatedResult<PayslipDto>>>;
}
