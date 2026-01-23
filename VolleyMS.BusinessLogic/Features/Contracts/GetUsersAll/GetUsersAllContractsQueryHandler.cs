using MediatR;
using VolleyMS.BusinessLogic.Features.Contracts.Get;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Contracts.GetUsersAll
{
    public sealed class GetUsersAllContractsQueryHandler : IRequestHandler<GetUsersAllContractsQuery, Result<PaginatedResult<ContractDto>>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        public GetUsersAllContractsQueryHandler(IContractRepository contractRepository, IUserRepository userRepository)
        {
            _contractRepository = contractRepository;
            _userRepository = userRepository;
        }
        public async Task<Result<PaginatedResult<ContractDto>>> Handle(GetUsersAllContractsQuery request, CancellationToken cancellationToken)
        {
            // Verify user exists
            var user = await _userRepository.GetByIdAsync(request.userId);
            if(user == null)
                return Result.Failure<PaginatedResult<ContractDto>>(DomainErrors.User.UserNotFound);

            // Get paged contracts
            var contractsResult = await _contractRepository.GetUserContractsPaged(request.userId, request.pageNumber, request.pageSize, cancellationToken);

            // Map to DTOs
            var contractDtos = contractsResult.Items.Select(c => new ContractDto
            {
                Id = c.Id,
                MemberId = c.UserId,
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
