using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs;
using VolleyMS.BusinessLogic.Features.Users.Get;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Users.Get
{
    public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameAsync(query.userName);

            if (user == null)
                return Result.Failure<UserDto>(DomainErrors.User.UserNotFound);

            UserDto userDto = new UserDto();

            if (query.requestorId == null || query.requestorId != user.Id)
            {
                userDto.Id = user.Id;
                userDto.UserName = user.UserName;
                userDto.FirstName = user.Name;
                userDto.LastName = user.Surname;
                userDto.AvatarUrl = user.AvatarUrl;
            }
            else
            {
                userDto.Id = user.Id;
                userDto.UserName = user.UserName;
                userDto.FirstName = user.Name;
                userDto.LastName = user.Surname;
                userDto.Email = user.Email;
                userDto.AvatarUrl = user.AvatarUrl;
                userDto.createdAt = user.CreatedAt;
                userDto.clubDtos = user.UserClubs.Select(uc => new UserClubDto
                {
                    JoinDate = uc.CreatedAt,
                    UserId = user.Id,
                    ClubId = uc.Club.Id,
                    ClubName = uc.Club.Name,
                    Role = uc.ClubMemberRole
                }).ToList();
            }

            return Result.Success(userDto);
        }
    }
}
