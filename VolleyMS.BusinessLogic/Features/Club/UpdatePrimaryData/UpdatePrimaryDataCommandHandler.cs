using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Club.UpdatePrimaryData
{
    public sealed class UpdatePrimaryDataCommandHandler : IRequestHandler<UpdatePrimaryDataCommand, Result>
    {
        private readonly IClubRepository _clubRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePrimaryDataCommandHandler(IClubRepository clubRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _clubRepository = clubRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdatePrimaryDataCommand request, CancellationToken cancellationToken)
        {
            // Check user existence
            var User = await _userRepository.GetByIdWithClubsAsync(request.RequestorId);
            if (User is null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            // Check club existence
            var Club = await _clubRepository.GetByIdAsync(request.ClubId);
            if (Club is null)
                return Result.Failure(DomainErrors.Club.ClubNotFound);

            // Check user permission
            var ClubRole = User.GetRoleInClub(request.ClubId); // if not in club, returns domain error 

            if (!ClubRoleConfig.HasPermission(ClubRole.Value, ClubPermission.EditClubDetails))
                return Result.Failure(DomainErrors.Club.InvalidPermission);

            // Update primary data
            var UpdateResult = Club.ChangePrimaryData(request.Name, request.Description);

            if(UpdateResult.IsFailure)
                return Result.Failure(UpdateResult.Error);

            // Save changes
            await _clubRepository.UpdateAsync(Club);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
