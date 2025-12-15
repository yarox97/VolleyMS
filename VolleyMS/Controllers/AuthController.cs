using MediatR;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Users.Authorisation.Registration;
using VolleyMS.BusinessLogic.Features.Users.Authorisation.Authentication;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly ISender Sender;
        public authController(ISender sender)
        {
            Sender = sender;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Registration([FromBody] RegistrationCommand command, CancellationToken cancellation)
        {
            var result = await Sender.Send(command, cancellation);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authorization([FromBody] LoginCommand command, CancellationToken cancellation)
        {
            var result = await Sender.Send(command, cancellation);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
