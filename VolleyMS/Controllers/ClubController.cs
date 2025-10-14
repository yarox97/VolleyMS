using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Contracts;
using VolleyMS.Core.Models;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly ClubService _clubService;
        public ClubController(ClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpPost("Create")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateClubRequest CreateClubRequest)
        {
            var club = Club.Create(new Guid(), CreateClubRequest.Name, "", CreateClubRequest.Description, CreateClubRequest.AvatarURL, CreateClubRequest.BackGroundURL);
            if (club.error == string.Empty)
            {
                try
                {
                    await _clubService.Create(club.club);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(club.error);
        }

        [HttpPut("AddUser")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddUser([FromBody] AddUserToClubRequest addUserToClubRequest)
        {
            try
            {
                await _clubService.AddUser(addUserToClubRequest.UserName, addUserToClubRequest.JoinCode);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
