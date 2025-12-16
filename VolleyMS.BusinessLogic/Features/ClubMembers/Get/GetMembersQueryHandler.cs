using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.ClubMembers.Get
{
    public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, Result<IEnumerable<ClubMemberDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;

        public GetMembersQueryHandler(IUserRepository userRepository, IClubRepository clubRepository)
        {
            _userRepository = userRepository;
            _clubRepository = clubRepository;
        }
        public async Task<Result<IEnumerable<ClubMemberDto>>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
        {
            var club = await _clubRepository.GetByIdAsync(request.clubId);
            if (club == null)
                return Result.Failure<IEnumerable<ClubMemberDto>>(DomainErrors.Club.ClubNotFound);

            var requestor = await _userRepository.GetByIdAsync(request.requestorId);
            if (requestor == null)
                return Result.Failure<IEnumerable<ClubMemberDto>>(DomainErrors.User.UserNotFound);

            var requestorRoleInClub = requestor.GetRoleInClub(request.clubId);
            if (requestorRoleInClub.IsFailure)
                return Result.Failure<IEnumerable<ClubMemberDto>>(DomainErrors.Club.InvalidPermission);

            var memberships = await _clubRepository.GetMembershipsAsync(request.clubId);

            var clubMemberDtos = memberships.Select(uc => new ClubMemberDto
            {
                UserId = uc.User?.Id ?? Guid.Empty,
                UserName = uc.User?.UserName ?? "Unknown",
                UserFirstName = uc.User?.Name ?? string.Empty,
                UserSurname = uc.User?.Surname ?? string.Empty,
                AvatarUrl = uc.User?.AvatarUrl,

                Role = uc.ClubMemberRole,
                JoinDate = uc.CreatedAt
            });

            return Result.Success(clubMemberDtos);
        }
    }
}
