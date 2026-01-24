using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.ClubMembers.Kick
{
    public sealed class KickMemberCommandHandler : IRequestHandler<KickMemberCommand, Result>
    {
        private readonly IClubRepository _clubRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserClubRepository _userClubRepository;
        private readonly IUnitOfWork _unitOfWork;
        public KickMemberCommandHandler(IClubRepository clubRepository, IUserRepository userRepository, IUserClubRepository userClubRepository, IUnitOfWork unitOfWork)
        {
            _clubRepository = clubRepository;
            _userRepository = userRepository;
            _userClubRepository = userClubRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(KickMemberCommand request, CancellationToken cancellationToken)
        {
            // Check user existence
            var member = await _userRepository.GetByIdAsync(request.memberId);
            if (member is null) 
                return Result.Failure(DomainErrors.User.UserNotFound);

            // Get club with members
            var club = await _clubRepository.GetByIdWithMembersAsync(request.clubId, cancellationToken);
            if (club is null) 
                return Result.Failure(DomainErrors.Club.ClubNotFound);
            
            // Check responser existence
            var responser = await _userRepository.GetByIdWithClubsAsync(request.responserId);
            if (responser is null) 
                return Result.Failure(DomainErrors.User.UserNotFound);

            // Check if responser is member of the club and has permission to kick members
            if (responser.GetRoleInClub(club.Id).IsFailure)
                return Result.Failure(DomainErrors.Club.InvalidPermission);
            else
            {
                var responserRole = responser.GetRoleInClub(club.Id).Value;
                if (!ClubRoleConfig.HasPermission(responserRole, ClubPermission.KickMember))
                    return Result.Failure(DomainErrors.Club.InvalidPermission);
            }

            // Kick member
            var kickMemberResult = club.RemoveMember(member);
            if (kickMemberResult.IsFailure)
                return Result.Failure(kickMemberResult.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
