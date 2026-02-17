using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.GetClubAll
{
    public record GetClubTasksQuery(
        Guid ClubId,
        Guid UserId,
        int Page = 1,
        int PageSize = 10) : IRequest<Result<PaginatedResult<TaskDto>>>;
}
