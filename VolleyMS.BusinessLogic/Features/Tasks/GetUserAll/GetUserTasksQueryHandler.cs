using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.GetUserAll
{
    public sealed class GetUserTasksQueryHandler : IRequestHandler<GetUserTasksQuery, Result<PaginatedResult<TaskDto>>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetUserTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Result<PaginatedResult<TaskDto>>> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var pagedTasks = await _taskRepository.GetTasksByUserIdAsync(
                request.UserId,
                page,
                pageSize,
                cancellationToken);

            var dtos = pagedTasks.Items.Select(t => t.ToDto()).ToList();

            var result = new PaginatedResult<TaskDto>(
                dtos,
                pagedTasks.PageNumber,
                pagedTasks.PageSize,
                pagedTasks.TotalCount);

            return Result.Success(result);
        }
    }
}
