using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Requests;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clubController : ControllerBase
    {
        private readonly ClubService _clubService;
        private readonly NotificationService _notificationService;
        public clubController(ClubService clubService, NotificationService notificationService)
        {
            _clubService = clubService;
            _notificationService = notificationService;
        }

        [HttpGet("{clubId}")]
        public async Task<IActionResult> GetClub(Guid clubId)
        {
            try
            {
                return Ok(await _clubService.GetById(clubId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
                await _clubService.AddMember(creatorId, clubId, creatorId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{clubId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid clubId)
        {
            try
            {
                await _clubService.Delete(clubId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{clubId}/members")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AcceptUserJoinRequest([FromBody] AddUserToClubRequest addUserToClubRequest)
        {
            var responseUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (responseUserIdClaim == null)
            {
                return BadRequest("Claims not found, try re-loggin");
            }

            if (!Guid.TryParse(responseUserIdClaim, out var responseUserId))
            {
                return BadRequest("Invalid sender Id claims");
            }

            try
            {
                await _clubService.AddMember(addUserToClubRequest.UserId, addUserToClubRequest.ClubId, responseUserId);
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
            var responseUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (responseUserIdClaim == null)
            {
                return BadRequest("Claims not found, try re-loggin");
            }

            if (!Guid.TryParse(responseUserIdClaim, out var responseUserId))
            {
                return BadRequest("Invalid sender Id claims");
            }

            try
            {
                // Delete member from the club
                await _clubService.DeleteMember(clubId, userId, responseUserId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
