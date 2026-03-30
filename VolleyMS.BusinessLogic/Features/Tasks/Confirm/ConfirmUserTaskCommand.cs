using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Confirm
{
    public record ConfirmUserTaskCommand(
        Guid UserTaskId,
        Guid RequestorId,
        TaskStatus FinalStatus, 
        bool ApplyPenalty 
    ) : IRequest<Result>;
}
