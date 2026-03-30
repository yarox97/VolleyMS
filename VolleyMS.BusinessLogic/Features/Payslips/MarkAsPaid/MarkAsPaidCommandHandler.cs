using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.MarkAsPaid
{
    public class MarkAsPaidCommandHandler : IRequestHandler<MarkAsPaidCommand, Result>
    {
        private readonly IPayslipRepository _payslipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkAsPaidCommandHandler(
            IPayslipRepository payslipRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _payslipRepository = payslipRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(MarkAsPaidCommand request, CancellationToken cancellationToken)
        {
            // get payslip with details to check permissions and update status
            var payslip = await _payslipRepository.GetPayslipByIdWithDetailsAsync(request.payslipId, cancellationToken);
            if (payslip == null)
            {
                return Result.Failure(new Error("Payslip.NotFound", "Payslip not found"));
            }

            // get user with clubs to check permissions
            var user = await _userRepository.GetByIdWithClubsAsync(request.userId);
            if (user == null)
            {
                return Result.Failure(DomainErrors.User.UserNotFound);
            }

            // check permissions
            var userRoleResult = user.GetRoleInClub(payslip.Contract.ClubId);
            bool hasPermission = userRoleResult.IsSuccess &&
                                 ClubRoleConfig.HasPermission(userRoleResult.Value, ClubPermission.ViewContracts);

            if (!hasPermission)
            {
                return Result.Failure(DomainErrors.Club.InvalidPermission);
            }

            // mark as paid
            var markResult = payslip.MarkAsPaid();
            if (markResult.IsFailure)
            {
                return markResult;
            }

            // save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}