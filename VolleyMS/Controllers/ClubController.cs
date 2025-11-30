using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Contracts.DTOs;
using VolleyMS.Core.Requests;
using VolleyMS.Extensions;

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
            var club = await _clubService.Get(clubId);
            var response = new ClubDto()
            {
                Id = club.Id,
                Name = club.Name,
                Description = club.Description,
                AvatarURL = club.AvatarURL,
                BackGroundURL = club.BackGroundURL,
                CreatedAt = club.CreatedAt,
                JoinCode = club.JoinCode
            };
            return Ok(response);
        }

        [HttpPost()]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateClubRequest CreateClubRequest)
        {
            var creatorId = User.GetUserId();
            
            CreateClubRequest.CreatorId = creatorId;
            var clubId = await _clubService.Create(CreateClubRequest);

            return Ok(await _clubService.AddMember(clubId, creatorId, ClubMemberRole.President));
        }

        [HttpDelete("{clubId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid clubId)
        {
            await _clubService.Delete(clubId);
            return Ok();
        }

        [HttpPut("{clubId}/members")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AcceptUserJoinRequest([FromBody] AddUserToClubRequest addUserToClubRequest)
        {
            var responseUser = User.GetUserId();

            await _clubService.AddMember(addUserToClubRequest.ClubId, addUserToClubRequest.UserId, addUserToClubRequest.clubMemberRole);
            return Ok();
        }

        [HttpGet("{clubId}/members/")]
        //[Authorize]
        public async Task<IActionResult> GetClubMembers(Guid clubId)
        {
            var members = await _clubService.GetAllUsers(clubId);
            return Ok(members);
        }

        [HttpDelete("{clubId}/members/{userId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteMember(Guid clubId, Guid userId)
        {
            var responseUserId = User.GetUserId();

            // Delete member from the club
            return Ok(await _clubService.DeleteMember(clubId, userId, responseUserId));
        }
    }
}
