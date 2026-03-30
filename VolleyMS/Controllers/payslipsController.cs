using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Payslips.GetAdjustments;
using VolleyMS.BusinessLogic.Features.Payslips.GetClubs;
using VolleyMS.BusinessLogic.Features.Payslips.GetUsers;
using VolleyMS.BusinessLogic.Features.Payslips.MarkAsPaid;
using VolleyMS.BusinessLogic.Features.Payslips.RevertAdjustments;
using VolleyMS.Extensions;

namespace VolleyMS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class payslipsController : ApiController
    {
        public payslipsController(ISender sender)
            : base(sender)
        {
        }

        [HttpGet("contract/{contractId}/my")]
        [Authorize]
        public async Task<IActionResult> GetMyContractPayslips([FromRoute] Guid contractId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetUserPayslipsQuery(User.GetUserId(), page, pageSize, contractId);

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyPayslips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetUserPayslipsQuery(User.GetUserId(), page, pageSize);

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("club/{clubId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetClubPayslips(Guid clubId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetClubsPayslipsQuery(clubId, User.GetUserId(), page, pageSize);

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("contract/{payslipId}")]
        [Authorize]
        public async Task<IActionResult> GetPayslipAdjustments(Guid payslipId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPayslipAdjustmentsQuery(payslipId, User.GetUserId(), page, pageSize);

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("adjustments/revert")]
        [Authorize]
        public async Task<IActionResult> RevertAdjustments([FromBody] IEnumerable<Guid> adjustmentsIds)
        {
            var command = new RevertAdjustmentsCommand(adjustmentsIds, User.GetUserId());

            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{payslipId}/mark-as-paid")]
        [Authorize]
        public async Task<IActionResult> MarkAsPaid([FromRoute] Guid payslipId)
        {
            var command = new MarkAsPaidCommand(payslipId, User.GetUserId());

            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
