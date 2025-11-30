using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Authorisation;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Requests;



namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly AuthService _authService;
        public authController(UserService userService, AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequest registerRequest)
        {
            await _authService.Register(registerRequest.userName, registerRequest.password, registerRequest.name, registerRequest.surname);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authorization([FromBody] LoginRequest loginRequest)
        {
            return Ok(await _authService.Authorize(loginRequest.userName, loginRequest.password));
        }
    }
}
