using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Entities;
using VolleyMS.Core.Models;
using VolleyMS.Contracts.DTOs;

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
            var senderIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (senderIdClaim == null)
            {
                return BadRequest("Claims not found, try re-loggin");
            }

            if (!Guid.TryParse(senderIdClaim, out var senderId))
            {
                return BadRequest("Invalid sender Id claims");
            }

            try
            {
                return Ok(await _notificationService.SendNotification(notificationRequest, senderId));
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
