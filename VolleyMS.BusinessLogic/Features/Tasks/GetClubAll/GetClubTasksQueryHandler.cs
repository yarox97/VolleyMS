using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.GetClubAll
{
    public sealed class GetClubTasksQueryHandler
        : IRequestHandler<GetClubTasksQuery, Result<PaginatedResult<TaskDto>>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public GetClubTasksQueryHandler(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<PaginatedResult<TaskDto>>> Handle(
            GetClubTasksQuery request,
            CancellationToken cancellationToken)
        {
            int page = request.Page <= 0 ? 1 : request.Page;
            int pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            
            var user = await _userRepository.GetByIdWithClubsAsync(request.UserId);
            if (user == null)
            {                 
                return Result.Failure<PaginatedResult<TaskDto>>(DomainErrors.User.UserNotFound);
            }

            var userRoleResult = user.GetRoleInClub(request.ClubId);
            if (userRoleResult.IsFailure || userRoleResult?.Value == null)
            {
                return Result.Failure<PaginatedResult<TaskDto>>(DomainErrors.Club.MemberNotFound);
            }
            var userRole = userRoleResult.Value;

            if (!ClubRoleConfig.HasPermission(userRole, ClubPermission.ViewTasks))
            {
                return Result.Failure<PaginatedResult<TaskDto>>(DomainErrors.Club.InvalidPermission);
            }


            var pagedTasks = await _taskRepository.GetTasksByClubIdAsync(
                request.ClubId,
                page,
                pageSize,
                cancellationToken);

            var dtos = pagedTasks.Items
                .Select(task => task.ToDto())
                .ToList();

            var result = new PaginatedResult<TaskDto>(
                items: dtos,
                pageNumber: pagedTasks.PageNumber,
                pageSize: pagedTasks.PageSize,
                totalCount: pagedTasks.TotalCount
            );

            return Result.Success(result);
        }
    }
}
