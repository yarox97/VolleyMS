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
        [Authorize]
        public async Task<IActionResult> Get(string userName)
        {
            var userId = User.GetUserId();
            if(userId == Guid.Empty) return Unauthorized();

            var result = await Sender.Send(new GetUserQuery(userName, userId));

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{userName}")]
        [Authorize]
        public async Task<IActionResult> Modify(string userName  )
        {
            return Ok();
        }
    }
}
