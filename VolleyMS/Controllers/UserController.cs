using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolleyMS.BusinessLogic.Features.Users.UpdateAttributes;
using VolleyMS.BusinessLogic.Features.Users.ChangeEmail;
using VolleyMS.BusinessLogic.Features.Users.Get;
using VolleyMS.BusinessLogic.Features.Users.Modify;
using VolleyMS.Extensions;
using VolleyMS.Web.Controllers;


namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ApiController
    {
        public userController(ISender sender)
            : base(sender)
        {
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> Get(string userName)
        {
            var userId = User.GetUserIdOrNull();

            var result = await Sender.Send(new GetUserQuery(userName, userId));

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userName = User.GetUserName();
            var userId = User.GetUserId();

            var result = await Sender.Send(new GetUserQuery(userName, userId));

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("modify-primary")]
        [Authorize]
        public async Task<IActionResult> ModifyPrimaryData([FromBody] ModifyUserCommand command)
        {
            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok() : HandleFailure(result);
        }

        [HttpPut("modify-email")]
        [Authorize]
        public async Task<IActionResult> ModifyEmail([FromBody] string? Email)
        {
            var command = new ChangeEmailCommand(User.GetUserId(), Email);

            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok() : HandleFailure(result);
        }

        [HttpPut("modify-attributes")]
        public async Task<IActionResult> UpdateAttributes([FromBody] UpdateAttributesDto request, CancellationToken cancellation)
        {
            var userId = User.GetUserId();
            if(userId == null)
            {
                return Unauthorized();
            }

            var command = new UpdatePlayerAttributesCommand(
                userId,
                request.Position,
                request.HeightCm,
                request.WeightKg,
                request.DominantHand,
                request.SpikeReachCm,
                request.BlockReachCm
            );

            var result = await Sender.Send(command, cancellation);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
