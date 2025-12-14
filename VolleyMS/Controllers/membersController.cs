using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            // Query
            return Ok();
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
