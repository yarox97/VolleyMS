using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Tasks.Create;
using VolleyMS.Core.Requests;
using VolleyMS.Extensions;

namespace VolleyMS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tasksController : ApiController
    {
        public tasksController(ISender sender)
            : base(sender)
        {
        }

        [HttpPost("club/{ClubId}/create")]
        [Authorize]
        public async Task<IActionResult> CreateTask([FromBody] TaskRequest request, [FromRoute] Guid ClubId, CancellationToken cancellation)
        {
            var command = new CreateTaskCommand(request, User.GetUserId(), ClubId);

            var result = await Sender.Send(command, cancellation);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
