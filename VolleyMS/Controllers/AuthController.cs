using MediatR;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Users.Authorisation.Registration;
using VolleyMS.BusinessLogic.Features.Users.Authorisation.Authentication;
using VolleyMS.BusinessLogic.Features.Users.Authorisation.LoginCheck;
using VolleyMS.BusinessLogic.Features.Users.Authorisation.EmailCheck;
using VolleyMS.Web.Controllers;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ApiController
    {
        public authController(ISender sender) : base(sender)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registration([FromBody] RegistrationCommand command, CancellationToken cancellation)
        {
            var result = await Sender.Send(command, cancellation);
            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result); 
        }

        [HttpGet("login-check")]
        public async Task<IActionResult> IsLoginFree([FromQuery] string userName, CancellationToken cancellation)
        {
            var result = await Sender.Send(new LoginCheckQuery(userName), cancellation);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("email-check")]
        public async Task<IActionResult> IsEmailFree([FromQuery] string email, CancellationToken cancellation)
        {
            var result = await Sender.Send(new EmailCheckQuery(email), cancellation);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Authorization([FromBody] LoginCommand command, CancellationToken cancellation)
        {
            var result = await Sender.Send(command, cancellation);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            var token = result.Value;

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(14)
            };

            Response.Cookies.Append("my-cookies", token, cookieOptions);

            return Ok();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("my-cookies");

            return Ok();
        }
    }
}
