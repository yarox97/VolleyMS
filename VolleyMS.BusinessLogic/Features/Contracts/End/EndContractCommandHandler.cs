using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Contracts.End
{
    public sealed class EndContractCommandHandler : IRequestHandler<EndContractCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EndContractCommandHandler(IUserRepository userRepository, IContractRepository contractRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _contractRepository = contractRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(EndContractCommand request, CancellationToken cancellationToken)
        {
            // Check user existence
            var currentUser = await _userRepository.GetByIdWithClubsAsync(request.userId);
            if (currentUser is null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            // Check contract existence
            var contract = await _contractRepository.GetByIdAsync(request.contractId);
            if (contract is null)
                return Result.Failure(DomainErrors.Contract.ContractNotFound);

            // Check permissions
            if (currentUser.Id == contract.AssignerId)
            {
                var roleInClub = currentUser.GetRoleInClub(contract.ClubId);

                if (roleInClub == null || !ClubRoleConfig.HasPermission(roleInClub.Value, ClubPermission.DeleteContract))
                {
                    return Result.Failure(DomainErrors.Club.InvalidPermission);
                }
            }

            // End contract
            var result = contract.EndContract(currentUser);

            if (result.IsFailure)
            {
                return result;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
