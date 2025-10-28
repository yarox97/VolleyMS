using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Models;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clubController : ControllerBase
    {
        private readonly ClubService _clubService;
        public clubController(ClubService clubService)
        {
            _clubService = clubService;
        }


        [HttpPost()]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateClubRequest CreateClubRequest)
        {
            var creatorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (creatorIdClaim == null)
            {
                return BadRequest("Claims not found, try re-loggin");
            }
            var club = Club.Create(new Guid(), CreateClubRequest.Name, "", CreateClubRequest.Description, CreateClubRequest.AvatarURL, CreateClubRequest.BackGroundURL);

            if (!Guid.TryParse(creatorIdClaim, out var creatorId))
            {
                return BadRequest("Invalid sender Id claims");
            }
            
            club.club.CreatorId = creatorId;
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

        [HttpPatch("addUser")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddUser([FromBody] AddUserToClubRequest addUserToClubRequest)
        {
            try
            {
                await _clubService.AddUser(addUserToClubRequest.UserId, addUserToClubRequest.JoinCode);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _clubService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
