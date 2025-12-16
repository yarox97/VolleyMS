using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.JoinClub.ApproveRequest;
using VolleyMS.BusinessLogic.Features.JoinClub.GetRequests;
using VolleyMS.BusinessLogic.Features.JoinClub.RejectRequest;
using VolleyMS.BusinessLogic.Features.JoinClub.RequestToJoinClub;
using VolleyMS.Extensions;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class joinClubRequestsController : ControllerBase
    {
        private readonly ISender Sender;
        public joinClubRequestsController(ISender sender)
        {
            Sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> RequestToJoinClub(
            [FromBody] JoinCodeDto joinCodeDto,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var command = new RequestToJoinClubCommand(userId, joinCodeDto.joinCode);

            var result = await Sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{clubId}")]
        [Authorize]
        public async Task<IActionResult> GetAllRequests(Guid clubId)
        {
            var userId = User.GetUserId();
            var query = new GetAllRequestsQuery(clubId, userId);

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{clubId}/requests/{requestId}/approve")]
        public async Task<IActionResult> ApproveRequest(
            [FromBody] ApproveRequestDto approveRequestDto, 
            [FromRoute] Guid clubId, 
            [FromRoute] Guid requestId, 
            CancellationToken cancellation)
        {
            var userId = User.GetUserId();

            var command = new ApproveRequestToJoinClubCommand(requestId, approveRequestDto.Role, userId);

            var result = await Sender.Send(command, cancellation);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{clubId}/requests/{requestId}/reject")]
        public async Task<IActionResult> RejectRequest([FromRoute] Guid clubId, [FromRoute] Guid requestId)
        {
            var responserId = User.GetUserId();

            var command = new RejectRequestToJoinClubCommand(requestId, responserId);

            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
