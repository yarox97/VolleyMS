using MediatR;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.Core.Requests;

namespace VolleyMS.Controllers
{
    [Route("api/clubs/{clubId}/join-requests")]
    [ApiController]
    public class joinClubController : ControllerBase
    {
        private readonly ISender Sender;
        public joinClubController(ISender sender)
        {
            Sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> RequestToJoinClub([FromBody] JoinClubRequest joinClubRequest)
        {
            // Command
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequests(Guid clubId)
        {
            // Query
            return Ok();
        }

        [HttpPost("{requestId}/approve")]
        public async Task<IActionResult> ApproveRequest(Guid clubId, Guid requestId, [FromBody] ApproveJoinClubRequest approveJoinClubRequest)
        {
            // Command
            return Ok();
        }

        [HttpPost("{requestId}/reject")]
        public async Task<IActionResult> RejectRequest(Guid clubId, Guid requestId)
        {
            // Command
            return Ok();
        }
    }
}
