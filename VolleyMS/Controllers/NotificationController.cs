using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Notifications.Get;
using VolleyMS.BusinessLogic.Features.Notifications.Check;
using VolleyMS.Core.Shared;
using VolleyMS.Extensions;
using VolleyMS.BusinessLogic.Features.Notifications.Delete;

namespace VolleyMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class notificationController : ControllerBase
    {
        private readonly ISender Sender;

        public notificationController(ISender sender)
        {
            Sender = sender;
        }

        [HttpGet("user/notifications")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<NotificationDto>>> GetNotifications([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetNotificationsQuery
            {
                userId = User.GetUserId(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("user/notifications/{notificationId}")]
        [Authorize]
        public async Task<IActionResult> Check(Guid notificationId)
        {
            var userId = User.GetUserId();

            var command = new CheckNotificationCommand(userId, notificationId);
            var result = await Sender.Send(command);
            
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [HttpDelete("user/notifications/{notificationId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid notificationId)
        {
            var userId = User.GetUserId();

            var command = new DeleteNotificationCommand(userId, notificationId);
            var result = await Sender.Send(command);

            return result.IsSuccess ? NoContent() : BadRequest(result?.Error);
        }
    }
}
