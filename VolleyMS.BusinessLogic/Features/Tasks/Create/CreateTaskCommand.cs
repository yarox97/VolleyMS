using MediatR;
using VolleyMS.Core.Shared;
using TaskRequest = VolleyMS.Core.Requests.TaskRequest;

namespace VolleyMS.BusinessLogic.Features.Tasks.Create
{
    public sealed record CreateTaskCommand(TaskRequest TaskRequest, Guid UserId, Guid ClubId) : IRequest<Result<Core.Models.Task>>;
}
