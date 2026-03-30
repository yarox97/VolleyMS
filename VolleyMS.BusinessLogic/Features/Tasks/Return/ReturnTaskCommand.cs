using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Return
{
    public sealed record ReturnTaskCommand(Guid userTaskId, Guid userId) : IRequest<Result>
    {
    }
}
