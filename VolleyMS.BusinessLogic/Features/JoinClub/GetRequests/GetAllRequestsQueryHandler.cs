using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.JoinClub.GetRequests
{
    public sealed class GetAllRequestsQueryHandler : IRequestHandler<GetAllRequestsQuery, Result<List<JoinClubRequestDto>>>
    {
        private readonly IClubRepository _clubRepository;
        public GetAllRequestsQueryHandler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }
        public async Task<Result<List<JoinClubRequestDto>>> Handle(GetAllRequestsQuery query, CancellationToken cancellationToken)
        {
            var aligedUsers = await _clubRepository.GetUsersByRole(query.clubId, ClubMemberRole.Creator, ClubMemberRole.President, ClubMemberRole.Coach);
            if (!aligedUsers.Any(au => au.Id == query.userId)) return Result.Failure<List<JoinClubRequestDto>>(DomainErrors.Club.InvalidPermission);

            var requests = await _clubRepository.GetAllJoinRequestsAsync(query.clubId, cancellationToken);

            var requestDtos = requests.Select(r => new JoinClubRequestDto(
                requestor: new UserDto
                {
                    Id = r.User.Id,
                    FirstName = r.User.Name,
                    LastName = r.User.Surname,
                    UserName = r.User.UserName,
                    Email = r.User.Email,
                    AvatarUrl = r.User.AvatarUrl
                },

                joinClubRequestStatus: r.JoinClubRequestStatus,
                createdAt: r.CreatedAt,
                respondedAt: r.UpdatedAt,

                responder: r.Responser != null ? new UserDto
                {
                    Id = r.Responser.Id,
                    FirstName = r.Responser.Name,
                    LastName = r.Responser.Surname,
                    UserName = r.Responser.UserName,
                    Email = r.Responser.Email,
                    AvatarUrl = r.Responser.AvatarUrl
                } : new UserDto()
            )).ToList();

            return Result.Success(requestDtos);
        }
    }
}
