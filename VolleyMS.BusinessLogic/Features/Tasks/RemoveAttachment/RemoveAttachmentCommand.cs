using MediatR;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.RemoveAttachment
{
    public sealed record RemoveTaskAttachmentCommand(
        Guid TaskId,
        Guid MediaFileId
    ) : IRequest<Result>;
    
}
