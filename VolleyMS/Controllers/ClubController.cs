using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Club.Create;
using VolleyMS.BusinessLogic.Features.Club.Delete;
using VolleyMS.BusinessLogic.Features.Club.Get;
using VolleyMS.Core.Requests;
using VolleyMS.Extensions;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clubController : ControllerBase
    {
        private readonly ISender Sender;
        public clubController(ISender sender)
        {
            Sender = sender;
        }

        [HttpGet("{clubId}")]
        public async Task<IActionResult> GetClub([FromRoute] Guid clubId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserIdOrNull(); // Get id from JWT session

            var query = new GetClubQuery(clubId, userId);
            var result = await Sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpPost()]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateClubCommand command, CancellationToken cancellationToken)
        {
            var result = await Sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpDelete("{clubId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid clubId)
        {
            var result =  await Sender.Send(new DeleteClubCommand(clubId, User.GetUserId()));

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
