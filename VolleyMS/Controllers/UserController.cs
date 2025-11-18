using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Contracts.DTOs;
using VolleyMS.Core.Entities;
using VolleyMS.Core.Requests;


namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly NotificationService _notificationService;
        private readonly JoinClubService _joinClubService;

        public userController(UserService userService, NotificationService notificationService, JoinClubService joinClubService)
        {
            _userService = userService;
            _notificationService = notificationService;
            _joinClubService = joinClubService;
        }

        [HttpGet("{userName}")]
        [Authorize]
        public async Task<IActionResult> Get(string userName)
        {
            try
            {
                var user = await _userService.Get(userName);
                var response = new UserDto()
                {
                    Id = user.Id,
                    FirstName = user.Name,
                    LastName = user.Surname,
                    UserName = user.UserName
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userName}")]
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

        [HttpPost("{userName}/clubs/join")]
        [Authorize]
        public async Task<IActionResult> RequestToJoinClub([FromRoute] string userName, [FromBody] JoinClubRequest joinClubRequest)
        {
            var userNameClaim = User.FindFirstValue(ClaimTypes.Name);
            if (userNameClaim == null)
            {
                return Unauthorized("Claims not found, try re-logging");
            }

            if(userNameClaim != userName)
            {
                return Forbid("Can't send join request on behalf of other users!");
            }

            try
            {
                await _joinClubService.RequestToJoinClubAsync(joinClubRequest.JoinCode, userName);
                return Ok("Request to join club was sent");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{userName}/notifications")]
        [Authorize]
        public async Task<ActionResult<IList<Notification>>> GetNotifications(string userName)
        {
            var userNameClaim = User.FindFirstValue(ClaimTypes.Name);
            if (userNameClaim == null)
            {
                return StatusCode(401, "Claims not found, try re-loggin");
            }

            if (userNameClaim != userName)
            {
                return StatusCode(403, "Can't check other user's notifications!");
            }

            var userNotifications = await _notificationService.GetNotifications(userName);
            var response = new List<NotificationDto>();

            foreach (var userNotification in userNotifications)
            {
                response.Add(new NotificationDto()
                {
                    Text = userNotification.Text,
                    notificationCategory = userNotification.NotificationType.notificationCategory,
                    LinkedUrl = userNotification.LinkedURL,
                    SenderId = userNotification.CreatorId,
                    IsChecked = userNotification.IsChecked
                });
            }

            return Ok(response);
        }

        [HttpPut("{userName}/notifications/{notificationId}")]
        [Authorize]
        public async Task<IActionResult> Check(Guid notificationId)
        {
            try
            {
                await _notificationService.Check(notificationId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
