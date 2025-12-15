using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.RequestToJoinClub
{
    public class RequestToJoinClubCommandHandler : IRequestHandler<RequestToJoinClubCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RequestToJoinClubCommandHandler(IUserRepository userRepository, IClubRepository clubRepository, IUnitOfWork unitOfWork)
        {
            _clubRepository = clubRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(RequestToJoinClubCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.requestorId);
            if (user == null) 
                return Result.Failure(DomainErrors.User.UserNotFound);

            var club = await _clubRepository.GetByJoinCodeAsync(command.joinCode);
            if (club == null) 
                return Result.Failure(DomainErrors.JoinClubRequest.ClubNotFound);

            var requestResult = club.RequestToJoinClub(user);
            if (requestResult.IsFailure) 
                return Result.Failure(requestResult.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
