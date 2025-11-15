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

            if (!Guid.TryParse(creatorIdClaim, out var creatorId))
            {
                return BadRequest("Invalid sender Id claims");
            }

            try
            {
                CreateClubRequest.CreatorId = creatorId;
                var clubId = await _clubService.Create(CreateClubRequest);
                await _clubService.AddMember(creatorId, clubId);
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

        [HttpPatch("{clubId}/members")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddMember([FromBody] AddUserToClubRequest addUserToClubRequest)
        {
            try
            {
                await _clubService.AddMember(addUserToClubRequest.UserId, addUserToClubRequest.ClubId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{clubId}/members/{userId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteMember(Guid clubId, Guid userId)
        {
            try
            {
                await _clubService.DeleteMember(clubId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
