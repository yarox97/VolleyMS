using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.GetAdjustments
{
    public class GetPayslipAdjustmentsQueryHandler : IRequestHandler<GetPayslipAdjustmentsQuery, Result<PaginatedResult<PayslipAdjustmentDto>>>
    {
        private readonly IPayslipRepository _payslipRepository;
        private readonly IUserRepository _userRepository;

        public GetPayslipAdjustmentsQueryHandler(IPayslipRepository payslipRepository, IUserRepository userRepository)
        {
            _payslipRepository = payslipRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<PaginatedResult<PayslipAdjustmentDto>>> Handle(GetPayslipAdjustmentsQuery request, CancellationToken cancellationToken)
        {
            // get user
            var user = await _userRepository.GetByIdWithClubsAsync(request.RequestorId);
            if (user == null)
            {
                return Result.Failure<PaginatedResult<PayslipAdjustmentDto>>(DomainErrors.User.UserNotFound);
            }

            var payslipAdjustments = await _payslipRepository.GetByPayslipIdWithDetailsAsync(request.PayslipId, request.page, request.pageSize, cancellationToken);

            // if null or empty, return empty list with total count 0
            if (payslipAdjustments == null || payslipAdjustments.TotalCount <= 0)
            {
                var emptyResult = new PaginatedResult<PayslipAdjustmentDto>(new List<PayslipAdjustmentDto>(), 0, request.page, request.pageSize);
                return Result.Success(emptyResult);
            }

            // second check if payslip is null, if so return empty list with total count 0
            var payslip = payslipAdjustments.Items.FirstOrDefault()?.Payslip;
            if (payslip == null)
            {
                var emptyResult = new PaginatedResult<PayslipAdjustmentDto>(new List<PayslipAdjustmentDto>(), 0, request.page, request.pageSize);
                return Result.Success(emptyResult);
            }

            // permissin check 
            bool isOwner = user.Id == payslip.Contract.UserId;
            bool hasManagerPermission = false;

            var userRoleResult = user.GetRoleInClub(payslip.Contract.ClubId);
            if (userRoleResult.IsSuccess && userRoleResult.Value != null)
            {
                hasManagerPermission = ClubRoleConfig.HasPermission(userRoleResult.Value, ClubPermission.ViewContracts);
            }

            if (!isOwner && !hasManagerPermission)
            {
                return Result.Failure<PaginatedResult<PayslipAdjustmentDto>>(DomainErrors.Club.InvalidPermission);
            }

            // map to dtos
            var dtos = payslipAdjustments.Items.Select(a => new PayslipAdjustmentDto
            {
                Id = a.Id,
                PayslipId = a.PayslipId,
                Amount = a.Amount,
                Type = a.Type.ToString(),
                Reason = a.Reason ?? string.Empty,
                SourceTaskId = a.SourceTaskId,
                Date = a.Date,
                IsReverted = a.IsReverted
            }).ToList();

            // create paginated result
            var paginatedDtos = new PaginatedResult<PayslipAdjustmentDto>(
                dtos,
                payslipAdjustments.TotalCount,
                request.page,
                request.pageSize);

            return Result.Success(paginatedDtos);
        }
    }
}