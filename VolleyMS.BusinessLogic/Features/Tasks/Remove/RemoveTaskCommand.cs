using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Remove
{ 
    public sealed record RemoveTaskCommand(
        Guid TaskId,
        Guid UserId,
        Guid ClubId
    ) : IRequest<Result>;

}
