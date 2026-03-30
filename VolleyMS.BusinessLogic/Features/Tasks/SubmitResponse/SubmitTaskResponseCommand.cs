using MediatR;
using Microsoft.AspNetCore.Http;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.SubmitResponse
{
    public record SubmitTaskResponseCommand(
        Guid TaskId,
        Guid UserId,
        string? Text,
        IList<IFormFile?> Files
    ) : IRequest<Result>;
}
