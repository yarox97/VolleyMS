using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Complete
{
    public sealed record CompleteTaskCommand(Guid UserTaskId, Guid UserId) : IRequest<Result>
    {
    }
}
