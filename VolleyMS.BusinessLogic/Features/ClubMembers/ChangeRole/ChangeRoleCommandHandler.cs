using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.ClubMembers.ChangeRole
{
    public sealed class ChangeRoleCommandHandler : IRequestHandler<ChangeRoleCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserClubRepository _userClubRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ChangeRoleCommandHandler(IUserRepository userRepository, IUserClubRepository userClubRepository, IUnitOfWork unitOfWork)
        {
            _userClubRepository = userClubRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
        {
            var requestor = await _userRepository.GetByIdWithClubsAsync(request.requestorId);
            if (requestor == null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            var responser = await _userRepository.GetByIdWithClubsAsync(request.responserId);
            if (responser == null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            var responserRole = responser.GetRoleInClub(request.clubId);
            if (responserRole.IsFailure || !ClubRoleConfig.HasPermission(responserRole.Value, ClubPermission.ChangeRole))
                return Result.Failure(DomainErrors.Club.InvalidPermission);

            if(responser == requestor)                 
                return Result.Failure(DomainErrors.Club.CannotChangeOwnRole);

            var userClub = await _userClubRepository.GetByIds(request.clubId, request.requestorId);
            if (userClub == null)
                return Result.Failure(DomainErrors.Club.ClubMembershipNotFound);


            var result = userClub.ChangeRole(request.newRole);
            if (result.IsFailure)
                return Result.Failure(result.Error);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
