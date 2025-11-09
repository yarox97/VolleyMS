using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Requests;


namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly UserService _userService;
        public userController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userName}")]
        [Authorize(Policy = "AdminAndCoach")]
        public async Task<IActionResult> Get(string userName)
        {
            try
            {
                return Ok(await _userService.Get(userName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{userName}")]
        [Authorize]
        public async Task<IActionResult> Modify(string userName, [FromBody] UserModificationRequest userModificationRequest)
        {
            var userNameClaim = User.FindFirstValue(ClaimTypes.Name);
            if (userNameClaim == null)
            {
                return StatusCode(401, "Claims not found, try re-loggin");
            }

            if (userNameClaim != userName)
            {
                return StatusCode(403, "Can't modify other users!");
            }
            try
            {
                await _userService.Modify(userName, userModificationRequest);
                return Ok("User was successfuly modified");
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}
