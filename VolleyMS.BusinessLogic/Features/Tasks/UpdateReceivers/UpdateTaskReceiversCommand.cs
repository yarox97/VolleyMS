using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.UpdateReceivers
{
    public sealed record UpdateTaskReceiversCommand(
        Guid TaskId,
        IEnumerable<Guid> NewReceiverIds
    ) : IRequest<Result>;
}
