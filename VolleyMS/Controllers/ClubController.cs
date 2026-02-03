using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Club.Create;
using VolleyMS.BusinessLogic.Features.Club.Delete;
using VolleyMS.BusinessLogic.Features.Club.Get;
using VolleyMS.BusinessLogic.Features.Club.UpdatePrimaryData;
using VolleyMS.Core.Requests;
using VolleyMS.Extensions;
using VolleyMS.Web.Controllers;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clubController : ApiController
    {
        public clubController(ISender sender)
            : base(sender)
        {
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

        [HttpPut("{clubId}")]
        [Authorize]
        public async Task<IActionResult> UpdatePrimaryData([FromRoute] Guid clubId, [FromBody] UpdatePrimaryDataDto updatePrimaryDataDto)
        {
            var command = new UpdatePrimaryDataCommand(clubId, User.GetUserId(), updatePrimaryDataDto.Description, updatePrimaryDataDto.Name);

            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{clubId}")]
        public async Task<IActionResult> Delete(Guid clubId)
        {
            var result =  await Sender.Send(new DeleteClubCommand(clubId, User.GetUserId()));

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
