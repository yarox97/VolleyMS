using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Contracts;
using VolleyMS.Core.Entities;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]NotificationRequest notificationRequest)
        {
            var senderIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (senderIdClaim == null)
            {
                return BadRequest("Claims not found, try re-loggin");
            }

            if(!Guid.TryParse(senderIdClaim, out var senderId))
            {
                return BadRequest("Invalid sender Id claims");
            }
            
            var notifTuple = Notification.Create(
                Guid.NewGuid(), 
                notificationRequest.NotificationType, 
                notificationRequest.IsChecked, 
                notificationRequest.Text, 
                notificationRequest.LinkedURL);
            try
            {
                return Ok(await _notificationService.SendNotification(
                    notifTuple.notification, 
                    notifTuple.error, 
                    senderId, 
                    notificationRequest.Receivers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
