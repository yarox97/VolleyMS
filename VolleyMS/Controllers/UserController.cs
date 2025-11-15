using AutoMapper.Execution;
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
        private readonly ClubService _clubService;
        private readonly NotificationService _notificationService;
        public userController(UserService userService, ClubService clubService, NotificationService notificationService)
        {
            _userService = userService;
            _clubService = clubService;
            _notificationService = notificationService;
        }

        [HttpGet("{userName}")]
        [Authorize]
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

        [HttpPost("/clubs")]
        [Authorize]
        public async Task<IActionResult> RequestToJoinClub(string joinCode)
        {

            var userNameClaim = User.FindFirstValue(ClaimTypes.Name);
            if (userNameClaim == null)
            {
                return StatusCode(401, "Claims not found, try re-loggin");
            }
            var user = await _userService.Get(userNameClaim);

            try
            {
                var club = await _clubService.GetClubByJoinCode(joinCode);
                var usersByRoleTmp = await _clubService.GetUsersIdsByRole(club.Id, ClubMemberRole.President);
                if (usersByRoleTmp == null)
                {
                    return BadRequest();
                }
                var targetUsersByRole = (IList<Guid>)usersByRoleTmp;
                await _notificationService.SendNotification(new NotificationRequest()
                {
                    NotificationCategory = NotificationCategory.ClubJoinRequest,
                    Receivers = targetUsersByRole,
                    Text = $"{user.Name} {user.Surname} requested to join your club {club.Name}",
                }, user.Id);

                return Ok("Request to join club was sent");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
