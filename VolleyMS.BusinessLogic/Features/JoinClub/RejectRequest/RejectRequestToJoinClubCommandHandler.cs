using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.RejectRequest
{
    public sealed class RejectRequestToJoinClubCommandHandler : IRequestHandler<RejectRequestToJoinClubCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RejectRequestToJoinClubCommandHandler(
            IUserRepository userRepository,
            IClubRepository clubRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _clubRepository = clubRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(RejectRequestToJoinClubCommand command, CancellationToken cancellationToken)
        {
            var joinRequest = await _clubRepository.GetJoinRequestByIdAsync(command.requestId, cancellationToken);

            if (joinRequest == null)
                return Result.Failure(DomainErrors.Club.JoinRequestNotFound);

            var club = joinRequest.Club;
            if (club == null)
                return Result.Failure(Error.NullValue);

            var responser = await _userRepository.GetByIdAsync(command.responserId);
            if (responser == null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            var rejectResult = club.RejectJoinClubRequest(joinRequest, responser);
            if (rejectResult.IsFailure)
                return Result.Failure(rejectResult.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
