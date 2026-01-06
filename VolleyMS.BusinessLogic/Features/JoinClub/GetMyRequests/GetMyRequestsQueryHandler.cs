
using MediatR;
using System.Net.Http.Headers;
using VolleyMS.BusinessLogic.Features.JoinClub.GetRequests;
using VolleyMS.BusinessLogic.Features.Users.Get;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;
using VolleyMS.DataAccess.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VolleyMS.BusinessLogic.Features.JoinClub.GetMyRequests
{
    public sealed class GetMyRequestsQueryHandler : IRequestHandler<GetMyRequestsQuery, Result<List<JoinClubRequestDto>>>
    {
        private readonly IJoinClubRepository _joinClubRepository;
        private readonly IUserRepository _userRepository;
        public GetMyRequestsQueryHandler(IJoinClubRepository joinClubRepository, IUserRepository userRepository)
        {
            _joinClubRepository = joinClubRepository;
            _userRepository = userRepository;
        }
        public async Task<Result<List<JoinClubRequestDto>>> Handle(GetMyRequestsQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(query.userId);
            if (user == null)
                return Result.Failure<List<JoinClubRequestDto>>(DomainErrors.User.UserNotFound);

            var requests = await _joinClubRepository.GetUserRequests(query.userId);

            if (requests == null || !requests.Any())
                return Result.Success(new List<JoinClubRequestDto>());

            var requestDtos = requests.Select(r => new JoinClubRequestDto(
                ClubId: r.Club.Id,         
                ClubName: r.Club.Name,
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
