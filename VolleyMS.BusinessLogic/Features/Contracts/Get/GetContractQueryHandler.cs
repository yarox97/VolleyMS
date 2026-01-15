using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Contracts.Get
{
    public sealed class GetContractQueryHandler : IRequestHandler<GetContractQuery, Result<ContractDto>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        public GetContractQueryHandler(IContractRepository contractRepository, IUserRepository userRepository, IClubRepository clubRepository)
        {
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _clubRepository = clubRepository;
        }
        public async Task<Result<ContractDto>> Handle(GetContractQuery request, CancellationToken cancellationToken)
        {
            // Get the contract requestor details
            var requestor = await _userRepository.GetByIdWithClubsAsync(request.requestorId);
            if (requestor is null)
                return Result.Failure<ContractDto>(DomainErrors.User.UserNotFound);

            // Get the contract
            var contract = await _contractRepository.GetByIdAsync(request.contractId);
            if(contract is null)
                return Result.Failure<ContractDto>(DomainErrors.Contract.ContractNotFound);

            // Verify that the requestor exists
            var member = await _userRepository.GetByIdAsync(contract.UserId);
            if (member is null)
                return Result.Failure<ContractDto>(DomainErrors.User.UserNotFound);

            // Check if requestor is either the contract owner or has permission to view contracts in the club
            var aligedToCheckContract = await _clubRepository.GetUsersByRole(contract.ClubId, ClubRoleConfig.GetRolesByPermission(ClubPermission.ViewContracts).ToArray());

            if (!(requestor.Id == contract.UserId || aligedToCheckContract.Any(u => u.Id == request.requestorId)))
                return Result.Failure<ContractDto>(DomainErrors.Contract.InvalidPermission);

            // Map to dto
            var contractDto = new ContractDto
            {
                StartDate = contract.BeginsFrom,
                EndDate = contract.EndsBy,
                Salary = contract.MonthlySalary,
                Currency = contract.Currency,
                AssignedAt = contract.CreatedAt,
                IsActive = contract.IsActive,

                ClubId = contract.Club.Id,
                ClubName = contract.Club.Name,

                MemberId = contract.User.Id,
                MemberName = contract.User.Name,
                MemberSurname = contract.User.Surname,

                ResponserName = requestor.Name,
                ResponserSurname = requestor.Surname,
                ResponserUserName = requestor.UserName,
            };

            return Result.Success(contractDto);
        }
    }
}
