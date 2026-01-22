using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Contracts.Assign;
using VolleyMS.BusinessLogic.Features.Contracts.End;
using VolleyMS.BusinessLogic.Features.Contracts.Get;
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

        [HttpPost("end-contract/{contractId}")]
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
    }
}
