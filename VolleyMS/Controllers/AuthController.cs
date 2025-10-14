using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Authorisation;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Contracts;


namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(UserService userService, AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequest registerRequest)
        {
            try
            {
                await _authService.Register(registerRequest.userName, registerRequest.password, registerRequest.name, registerRequest.surname);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Authorization([FromBody] Contracts.LoginRequest loginRequest)
        {
            try
            {
                return Ok(await _authService.Authorize(loginRequest.userName, loginRequest.password));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
