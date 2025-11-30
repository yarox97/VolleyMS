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
    public class notificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public notificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] NotificationRequest notificationRequest)
        {
            var senderId = User.GetUserId();

            return Ok(await _notificationService.SendNotification(notificationRequest, senderId));
        }


        [HttpGet("user/notifications")]
        [Authorize]
        public async Task<ActionResult<IList<NotificationDto>>> GetNotifications()
        {
            var userId = User.GetUserId();

            var userNotifications = await _notificationService.Get(userId);
            var response = new List<NotificationDto>();

            foreach (var userNotification in userNotifications)
            {
                response.Add(new NotificationDto
                {
                    Text = userNotification.Notification.Text,
                    IsChecked = userNotification.IsChecked,
                    LinkedURL = userNotification.Notification.LinkedURL,
                    NotificationCategory = userNotification.Notification.Category,
                    RequiredClubMemberRoles = userNotification.Notification.RequiredClubMemberRoles,
                    SenderId = userNotification.Notification.CreatorId,
                    PayLoad = userNotification.Notification.Payload,
                });
            }

            return Ok(response);
        }

        [HttpPut("user/notifications/{notificationId}")]
        [Authorize]
        public async Task<IActionResult> Check(Guid notificationId)
        {
            var userName = User.GetUserName();
            await _notificationService.Check(notificationId, userName);
            return NoContent();
        }

        [HttpDelete("user/notifications/{notificationId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid notificationId)
        {
            var userId = User.GetUserId();
            await _notificationService.DeleteUserNotification(notificationId, userId);
            return NoContent();
        }
    }
}
