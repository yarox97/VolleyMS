using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.ClubMembers.Get;
using VolleyMS.Extensions;

namespace VolleyMS.Web.Controllers
{
    [Route("api/{clubId}/[controller]")]
    [ApiController]
    public class membersController : ControllerBase
    {
        private readonly ISender Sender;
        public membersController(ISender sender)
        {
            Sender = sender;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetClubMembers(Guid clubId)
        {
            var query = new GetMembersQuery(clubId, User.GetUserId());
            
            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteMember(Guid clubId, Guid userId)
        {
            // Command
            return Ok();
        }
    }
}
