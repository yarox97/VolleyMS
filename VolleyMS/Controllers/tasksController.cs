using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.BusinessLogic.Features.Tasks.AddAttachment;
using VolleyMS.BusinessLogic.Features.Tasks.Complete;
using VolleyMS.BusinessLogic.Features.Tasks.Confirm;
using VolleyMS.BusinessLogic.Features.Tasks.Create;
using VolleyMS.BusinessLogic.Features.Tasks.GetClubAll;
using VolleyMS.BusinessLogic.Features.Tasks.GetDetails;
using VolleyMS.BusinessLogic.Features.Tasks.GetUserAll;
using VolleyMS.BusinessLogic.Features.Tasks.Remove;
using VolleyMS.BusinessLogic.Features.Tasks.Return;
using VolleyMS.BusinessLogic.Features.Tasks.SubmitResponse;
using VolleyMS.BusinessLogic.Features.Tasks.UpdateDetails;
using VolleyMS.BusinessLogic.Features.Tasks.UpdateReceivers;
using VolleyMS.Core.Requests;
using VolleyMS.Extensions;

namespace VolleyMS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tasksController : ApiController
    {
        public tasksController(ISender sender)
            : base(sender)
        {
        }

        [HttpPost("club/{ClubId}/create")]
        [Authorize]
        public async Task<IActionResult> CreateTask([FromForm] TaskRequest request, [FromRoute] Guid ClubId, CancellationToken cancellation)
        {
            var command = new CreateTaskCommand(request, User.GetUserId(), ClubId);

            var result = await Sender.Send(command, cancellation);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string filter = "all")
        {
            if (User.GetUserIdOrNull() == null) return Forbid();

            var query = new GetUserTasksQuery(User.GetUserId(), filter, page, pageSize);

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("club/{clubId}")]
        [Authorize]
        public async Task<IActionResult> GetClubTasks(Guid clubId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (User.GetUserIdOrNull() == null) return Forbid();

            var query = new GetClubTasksQuery(clubId, User.GetUserId(), page, pageSize);

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("{taskId}")]
        [Authorize]
        public async Task<IActionResult> GetTaskDetails(Guid taskId)
        {
            if (User.GetUserIdOrNull() == null) return Forbid();

            var query = new GetTaskDetailsQuery(taskId, User.GetUserId());

            var result = await Sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{taskId}/details")]
        [Authorize]
        public async Task<IActionResult> UpdateDetails(Guid taskId, [FromBody] UpdateTaskDetailsCommand command)
        {
            if (taskId != command.TaskId) return BadRequest("Id mismatch");

            var result = await Sender.Send(command);

            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [HttpPut("{taskId}/receivers")]
        [Authorize]
        public async Task<IActionResult> UpdateReceivers(Guid taskId, [FromBody] UpdateTaskReceiversRequest request)
        {
            var currentUserId = User.GetUserId();

            var validReceiverIds = request.NewReceiverIds
                .Where(id => id != currentUserId)
                .Distinct()
                .ToList();

            var command = new UpdateTaskReceiversCommand(
                taskId,
                validReceiverIds,
                currentUserId
            );

            var result = await Sender.Send(command);

            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [HttpDelete("club/{clubId}/tasks/{taskId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid clubId, Guid taskId)
        {
            var result = await Sender.Send(new RemoveTaskCommand(taskId, User.GetUserId(), clubId));

            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [HttpPost("{id}/attachments")]
        [Authorize]
        public async Task<IActionResult> AddAttachments(Guid id, [FromForm] ICollection<IFormFile> files)
        {
            var command = new AddTaskAttachmentCommand(id, files, User.GetUserId());

            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("{taskId}/respond")]
        [Authorize]
        public async Task<IActionResult> SubmitTaskResponse([FromRoute] Guid taskId, [FromForm] string? Text, [FromForm] IList<IFormFile?> Files)
        {
            var command = new SubmitTaskResponseCommand(taskId, User.GetUserId(), Text, Files);

            var result = await Sender.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{userTaskId}/complete")]
        [Authorize]
        public async Task<IActionResult> CompleteTask([FromRoute] Guid userTaskId)
        {
            var command = new CompleteTaskCommand(userTaskId, User.GetUserId());

            var result = await Sender.Send(command);

            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [HttpPut("{userTaskId}/return")]
        [Authorize]
        public async Task<IActionResult> ReturnTask([FromRoute] Guid userTaskId)
        {
            var command = new ReturnTaskCommand(userTaskId, User.GetUserId());

            var result = await Sender.Send(command);

            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [HttpPost("{userTaskId}/confirm")]
        [Authorize]
        public async Task<IActionResult> ConfirmTask([FromRoute] Guid userTaskId, [FromBody] ConfirmUserTaskCommandDto confirmUserTaskCommandDto)
        {
            var command = new ConfirmUserTaskCommand(userTaskId, User.GetUserId(), confirmUserTaskCommandDto.TaskStatus, confirmUserTaskCommandDto.ApplyPenalty);

            var result = await Sender.Send(command);

            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }
    }
}

