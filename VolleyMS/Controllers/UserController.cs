using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Services;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration(string userName, string Password, string Name, string Surname)
        {
            try
            {
                await _userService.Register(userName, Password, Name, Surname);
                return Ok(Password);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("authorisation")]
        //[Authorize]
        public async Task<IActionResult> Authorization(string userName, string password)
        {
            try
            {
                return Ok(await _userService.Authorize(userName, password));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
