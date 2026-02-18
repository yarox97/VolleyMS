using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.SubmitResponse
{
    public record SubmitTaskResponseCommand(
        Guid TaskId,
        Guid UserId,
        string? Text,
        List<AttachmentDto>? Files 
    ) : IRequest<Result>;
}
