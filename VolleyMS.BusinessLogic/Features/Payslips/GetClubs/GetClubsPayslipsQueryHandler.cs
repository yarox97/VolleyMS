using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.GetClubs
{
    public sealed class GetClubPayslipsQueryHandler : IRequestHandler<GetClubsPayslipsQuery, Result<PaginatedResult<PayslipDto>>>
    {
        private readonly IPayslipRepository _payslipRepository;
        private readonly IUserRepository _userRepository;

        public GetClubPayslipsQueryHandler(IPayslipRepository payslipRepository, IUserRepository userRepository)
        {
            _payslipRepository = payslipRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<PaginatedResult<PayslipDto>>> Handle(GetClubsPayslipsQuery request, CancellationToken cancellationToken)
        {
            // check user existance and permissions
            var requestor = await _userRepository.GetByIdWithClubsAsync(request.RequestorId);
            if (requestor == null)
                return Result.Failure<PaginatedResult<PayslipDto>>(DomainErrors.User.UserNotFound);

            var roleResult = requestor.GetRoleInClub(request.ClubId);
            if (roleResult.IsFailure || roleResult?.Value == null)
                return Result.Failure<PaginatedResult<PayslipDto>>(DomainErrors.Club.MemberNotFound);

            
            if (!ClubRoleConfig.HasPermission(roleResult.Value, ClubPermission.ViewContracts))
            {
                return Result.Failure<PaginatedResult<PayslipDto>>(DomainErrors.Club.InvalidPermission);
            }

            // get payslips by club id
            var paginatedPayslips = await _payslipRepository.GetByClubIdWithDetailsAsync(
                request.ClubId, request.Page, request.PageSize, cancellationToken);

            // map
            var dtoList = paginatedPayslips.Items.Select(p => new PayslipDto(
                Id: p.Id,
                Month: p.Month,
                Year: p.Year,
                BaseSalary: p.BaseSalary,
                TotalAmount: p.TotalAmount,
                Status: p.Status.ToString(),
                ClubName: p.Contract?.Club?.Name ?? "Unknown Club",
                UserId: p.UserId,
                UserFullName: p.User != null ? $"{p.User.Name} {p.User.Surname}" : "Unknown User"
            )).ToList();

            // return paginated result
            var paginatedResultDto = new PaginatedResult<PayslipDto>(
                dtoList,
                paginatedPayslips.TotalCount,
                paginatedPayslips.PageNumber,
                paginatedPayslips.PageSize
            );

            return Result.Success(paginatedResultDto);
        }
    }
}
