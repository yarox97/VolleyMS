using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Users.Get;
using VolleyMS.Extensions;


namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly ISender Sender;

        public userController(ISender sender)
        {
            Sender = sender;
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
    }
}
