using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Contracts.Assign;
using VolleyMS.BusinessLogic.Features.Contracts.End;
using VolleyMS.BusinessLogic.Features.Contracts.Get;
using VolleyMS.BusinessLogic.Features.Contracts.GetClubsAll;
using VolleyMS.BusinessLogic.Features.Contracts.GetUsersAll;
using VolleyMS.Extensions;

namespace VolleyMS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class contractsController : ControllerBase
    {
        private readonly ISender Sender;

        public contractsController(ISender sender)
        {
            Sender = sender;
        }

        [HttpPost("assign-contract")]
        [Authorize]
        public async Task<IActionResult> AssignContract([FromBody] AssignContractCommand command)
        {
            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("end-contract/{contractId}")]
        [Authorize]
        public async Task<IActionResult> EndContract([FromRoute] Guid contractId)
        {
            var result = await Sender.Send(new EndContractCommand(User.GetUserId(), contractId));

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{contractId}")]
        [Authorize]
        public async Task<IActionResult> GetContract([FromRoute] Guid contractId)
        {
            var query = new GetContractQuery(contractId, User.GetUserId());

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("user/my-contracts")]
        [Authorize]
        public async Task<IActionResult> GetMyContracts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetUsersAllContractsQuery(User.GetUserId(), pageNumber, pageSize);

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("club/{clubId}/contracts")]
        [Authorize]
        public async Task<IActionResult> GetClubContracts([FromRoute] Guid clubId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetClubsAllContractsQuery(clubId, pageSize, pageNumber, User.GetUserId());

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

    }
}
