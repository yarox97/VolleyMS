using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.RevertAdjustments
{
    public class RevertAdjustmentsCommandHandler : IRequestHandler<RevertAdjustmentsCommand, Result>
    {
        private readonly IPayslipRepository _payslipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RevertAdjustmentsCommandHandler(
            IPayslipRepository payslipRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _payslipRepository = payslipRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RevertAdjustmentsCommand request, CancellationToken cancellationToken)
        {
            var requestor = await _userRepository.GetByIdWithClubsAsync(request.userId);
            if (requestor == null)
            {
                return Result.Failure(DomainErrors.User.UserNotFound);
            }

            var adjustments = await _payslipRepository.GetAdjustmentsByIdsAsync(request.taskAdjustmentIds, cancellationToken);

            if (!adjustments.Any())
            {
                return Result.Failure(new Error("Adjustments.NotFound", "No adjustments found for the provided IDs."));
            }

            var adjustmentsByPayslip = adjustments.GroupBy(a => a.PayslipId);

            foreach (var payslipGroup in adjustmentsByPayslip)
            {
                var payslip = await _payslipRepository.GetPayslipByIdWithDetailsAsync(payslipGroup.Key, cancellationToken);
                if (payslip == null) continue;

                var userRoleResult = requestor.GetRoleInClub(payslip.Contract.ClubId);

                if (userRoleResult.IsFailure || userRoleResult.Value == null)
                {
                    return Result.Failure(DomainErrors.Club.MemberNotFound);
                }

                if (!ClubRoleConfig.HasPermission(userRoleResult.Value, ClubPermission.ViewContracts))
                {
                    return Result.Failure(DomainErrors.Club.InvalidPermission);
                }

                foreach (var adjustmentToRevert in payslipGroup)
                {
                    var revertResult = payslip.RevertAdjustment(adjustmentToRevert.Id);

                    if (revertResult.IsFailure)
                    {
                        return revertResult;
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}