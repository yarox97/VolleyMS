using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Requests;
using VolleyMS.Extensions;

namespace VolleyMS.Controllers
{
    [Route("api/clubs/{clubId}/join-requests")]
    [ApiController]
    public class joinClubController : ControllerBase
    {
        private readonly JoinClubService _joinClubService;

        public joinClubController(JoinClubService joinClubService)
        {
            _joinClubService = joinClubService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestToJoinClub([FromBody] JoinClubRequest joinClubRequest)
        {
            var userName = User.GetUserName();

            await _joinClubService.RequestToJoinClubAsync(joinClubRequest.JoinCode, userName);
            return Ok("Request to join club was sent");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequests(Guid clubId)
        {
            var requests = await _joinClubService.GetAll(clubId);
            return Ok(requests);
        }

        [HttpPost("{requestId}/approve")]
        public async Task<IActionResult> ApproveRequest(Guid clubId, Guid requestId, [FromBody] ApproveJoinClubRequest approveJoinClubRequest)
        {
            var responderUserId = User.GetUserId();
            return Ok(await _joinClubService.ApproveClubJoinRequest(clubId, requestId, approveJoinClubRequest.ClubMemberRole, responderUserId));
        }

        [HttpPost("{requestId}/reject")]
        public async Task<IActionResult> RejectRequest(Guid clubId, Guid requestId)
        {
            var responderUserId = User.GetUserId();
            return Ok(await _joinClubService.RejectClubJoinRequest(clubId, requestId, responderUserId));
        }
    }
}
