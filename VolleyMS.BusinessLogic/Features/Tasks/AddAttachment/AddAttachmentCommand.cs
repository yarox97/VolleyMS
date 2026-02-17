using MediatR;
using Microsoft.AspNetCore.Http;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.AddAttachment
{
    public sealed record AddTaskAttachmentCommand(
        Guid TaskId,
        ICollection<IFormFile> Files
    ) : IRequest<Result>;
}  

