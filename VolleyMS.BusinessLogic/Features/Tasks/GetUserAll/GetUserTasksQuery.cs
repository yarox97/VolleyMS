using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.GetUserAll
{
    public record GetUserTasksQuery(
    Guid UserId,
    string Filter,
    int Page = 1,
    int PageSize = 10
    ) : IRequest<Result<PaginatedResult<TaskDto>>>;
}
