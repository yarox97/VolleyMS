using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;
using Contract = VolleyMS.Core.Models.Contract;

namespace VolleyMS.BusinessLogic.Features.Contracts.Assign
{
    public sealed class AssignContractCommandHandler : IRequestHandler<AssignContractCommand, Result>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignContractCommandHandler(IContractRepository contractRepository, IUserRepository userRepository, IClubRepository clubRepository, IUnitOfWork unitOfWork)
        {
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _clubRepository = clubRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(AssignContractCommand request, CancellationToken cancellationToken)
        {
            // Check responser existence
            var responser = await _userRepository.GetByIdAsync(request.responserId);
            if (responser is null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            // Check user existence 
            var member = await _userRepository.GetByIdAsync(request.memberId);
            if (member is null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            // Check that responser is not assigning contract to self
            if (member.Id == responser.Id)
                return Result.Failure(DomainErrors.Contract.CannotAssignContractToSelf);

            // Check club existence
            var club = await _clubRepository.GetByIdWithMembersAsync(request.clubId, cancellationToken);
            if(club is null)
                return Result.Failure(DomainErrors.Club.ClubNotFound);

            //Check if the member is part of the club
            if (!club.HasMember(member.Id))
                return Result.Failure(DomainErrors.Club.MemberNotFound);

            // Check responser permissions
            if (!ClubRoleConfig.HasPermission(responser.GetRoleInClub(club.Id).Value, ClubPermission.CreateContract)) 
                return Result.Failure(DomainErrors.Club.InvalidPermission);

            // Check if the member has contract already
            var existingContract = await _contractRepository.GetActiveContractByUserAndClubAsync(member.Id, club.Id, cancellationToken);
            if (existingContract.IsSuccess)
                return Result.Failure(DomainErrors.Contract.ActiveContractAlreadyExists);

            // Create contract
            var contractResult = Contract.Create(
                request.monthlySalary,
                request.currency,
                request.beginsFrom,
                request.endsBy,
                member,
                club);

            if (contractResult.IsFailure) return contractResult;

            await _contractRepository.AddAsync(contractResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
