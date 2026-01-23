using MediatR;
using VolleyMS.BusinessLogic.Features.Contracts.Get;
using VolleyMS.BusinessLogic.Features.Contracts.GetUsersAll;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Contracts.GetClubsAll
{
    public sealed class GetClubAllContractsQueryHandler : IRequestHandler<GetClubsAllContractsQuery, Result<PaginatedResult<ContractDto>>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUserRepository _userRepository;
        public GetClubAllContractsQueryHandler(IContractRepository contractRepository,IClubRepository clubRepository, IUserRepository userRepository)
        {
            _contractRepository = contractRepository;
            _clubRepository = clubRepository;
            _userRepository = userRepository;
        }
        public async Task<Result<PaginatedResult<ContractDto>>> Handle(GetClubsAllContractsQuery request, CancellationToken cancellationToken)
        {
            // Verify club exists
            var club = await _clubRepository.GetByIdAsync(request.clubId);
            if (club == null)
                return Result.Failure<PaginatedResult<ContractDto>>(DomainErrors.Club.ClubNotFound);

            // Verify user exists   
            var user = await _userRepository.GetByIdWithClubsAsync(request.requestorId);
            if (user == null)
                return Result.Failure<PaginatedResult<ContractDto>>(DomainErrors.User.UserNotFound);

            // Verify user is member of the club
            var userRole = user.GetRoleInClub(club.Id);
            if (userRole.IsFailure)
                return Result.Failure<PaginatedResult<ContractDto>>(DomainErrors.Club.MemberNotFound);

            // Verify user has permission to view contracts
            if (!ClubRoleConfig.HasPermission(userRole.Value, ClubPermission.ViewContracts))
                return Result.Failure<PaginatedResult<ContractDto>>(DomainErrors.Club.InvalidPermission);

            // Get paged contracts
            var contractsResult = await _contractRepository.GetClubContractsPaged(request.clubId, request.pageNumber, request.pageSize, cancellationToken);


            // Map to DTOs
            var contractDtos = contractsResult.Items.Select(c => new ContractDto
            {
                Id = c.Id,
                MemberId = c.UserId,
                MemberName = c.User.Name,
                MemberSurname = c.User.Surname,
                ResponserName = c.Assigner.Name,
                ResponserSurname = c.Assigner.Surname,
                ClubId = c.ClubId,
                ClubName = c.Club.Name,
                StartDate = c.BeginsFrom,
                EndDate = c.EndsBy,
                AssignedAt = c.CreatedAt,
                IsActive = c.IsActive,
                Salary = c.MonthlySalary,
                Currency = c.Currency
            });

            // Return paginated result
            return Result.Success(new PaginatedResult<ContractDto>(
                contractDtos,
                contractsResult.TotalCount,
                request.pageNumber,
                request.pageSize));
        }
    }
}
