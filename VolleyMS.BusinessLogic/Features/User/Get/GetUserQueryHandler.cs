using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs;
using VolleyMS.BusinessLogic.Features.Users.Get;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;
using VolleyMS.Core.Enums; 

namespace VolleyMS.BusinessLogic.Features.Users.Get
{
    public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public GetUserQueryHandler(IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        public async Task<Result<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameWithClubsAsync(query.userName);

            if (user == null)
                return Result.Failure<UserDto>(DomainErrors.User.UserNotFound);

            var stats = await _taskRepository.GetUserTaskStatsAsync(user.Id, cancellationToken);

            int completionRate = stats.TotalTasks == 0
                ? 0
                : (int)Math.Round((double)stats.TasksFinished / stats.TotalTasks * 100);

            UserDto userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.Name,
                LastName = user.Surname,
                AvatarUrl = user.AvatarUrl,
                createdAt = user.CreatedAt,
                userType = user.UserType,

                MatchesPlayed = stats.MatchesPlayed,
                TasksFinished = stats.TasksFinished,
                TaskCompletionRate = completionRate,

                clubDtos = user.UserClubs.Select(uc => new UserClubDto
                {
                    JoinDate = uc.CreatedAt,
                    UserId = user.Id,
                    ClubId = uc.Club.Id,
                    ClubName = uc.Club.Name,
                    Role = uc.ClubMemberRole
                }).ToList(),

                Attributes = user.Attributes != null ? new PlayerAttributesDto
                {
                    Position = user.Attributes.Position?.ToString(),
                    HeightCm = user.Attributes.HeightCm,
                    WeightKg = user.Attributes.WeightKg,
                    DominantHand = user.Attributes.DominantHand?.ToString(),
                    SpikeReachCm = user.Attributes.SpikeReachCm,
                    BlockReachCm = user.Attributes.BlockReachCm
                } : null
            };

            if (query.requestorId != null && query.requestorId == user.Id)
            {
                userDto.Email = user.Email;
            }

            return Result.Success(userDto);
        }
    }
}