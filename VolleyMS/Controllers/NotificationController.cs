using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Notifications.Get;
using VolleyMS.Extensions;

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
        public async Task<ActionResult<IList<NotificationDto>>> GetNotifications()
        {
            var query = new GetNotificationsQuery(User.GetUserId());

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        //[HttpPut("user/notifications/{notificationId}")]
        //[Authorize]
        //public async Task<IActionResult> Check(Guid notificationId)
        //{
        //    var userName = User.GetUserName();

        //    return NoContent();
        //}

        //[HttpDelete("user/notifications/{notificationId}")]
        //[Authorize]
        //public async Task<IActionResult> Delete(Guid notificationId)
        //{
        //    var userId = User.GetUserId();

        //    return NoContent();
        //}
    }
}
