using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.ApproveRequest
{
    public class ApproveRequestToJoinClubCommandHandler : IRequestHandler<ApproveRequestToJoinClubCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveRequestToJoinClubCommandHandler(
            IUserRepository userRepository,
            IClubRepository clubRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _clubRepository = clubRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ApproveRequestToJoinClubCommand command, CancellationToken cancellationToken)
        {
            var joinRequest = await _clubRepository.GetJoinRequestByIdAsync(command.joinClubRequestId, cancellationToken);

            if (joinRequest == null)
                return Result.Failure(DomainErrors.Club.JoinRequestNotFound);

            var club = joinRequest.Club;
            if (club == null)
                return Result.Failure(Error.NullValue);

            var responser = await _userRepository.GetByIdAsync(command.responserId);
            if (responser == null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            if (!Enum.IsDefined(typeof(ClubMemberRole), command.clubMemberRole))
            {
                return Result.Failure(DomainErrors.Club.InvalidClubMemberRole);
            }

            var approveResult = club.ApproveJoinRequest(joinRequest, command.clubMemberRole, responser);

            if (approveResult.IsFailure)
                return Result.Failure(approveResult.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}