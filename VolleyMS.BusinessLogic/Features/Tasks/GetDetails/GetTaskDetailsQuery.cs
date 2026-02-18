using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.GetDetails
{
    public sealed record GetTaskDetailsQuery(Guid TaskId, Guid UserId) : IRequest<Result<TaskDetailsDto>>
    {
    }
}
