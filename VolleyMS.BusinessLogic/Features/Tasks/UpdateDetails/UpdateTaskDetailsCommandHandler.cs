using MediatR;
using VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.UpdateDetails
{
    public class UpdateTaskDetailsCommandHandler : IRequestHandler<UpdateTaskDetailsCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskDetailsCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateTaskDetailsCommand command, CancellationToken ct)
        {
            var task = await _taskRepository.GetByIdAsync(command.TaskId);
            if (task is null)
                return Result.Failure(DomainErrors.Task.TaskNotFound);

            // 1. Создаем новое расписание (Value Object)
            Result<TaskSchedule> scheduleResult;
            if (command.DaysOfWeek != null && command.DaysOfWeek.Any())
            {
                scheduleResult = TaskSchedule.CreateReccuring(
                    command.StartDate, command.EndDate, command.DaysOfWeek, command.StartTime, command.EndTime);
            }
            else
            {
                scheduleResult = TaskSchedule.CreateOneTime(
                    command.StartDate, command.StartTime, command.EndTime);
            }

            if (scheduleResult.IsFailure)
                return Result.Failure(scheduleResult.Error);

            var payloadResult = TaksPayloadFactory.CreatePayLoad(command.ToTaskRequest(), task.SenderId);

            var updateResult = task.UpdateDetails(
                command.Title,
                command.Description,
                command.TaskPriority,
                command.PenaltyType,
                scheduleResult.Value,
                payloadResult?.Value // new payload, if it was successfully created, otherwise keep the old one
            );

            if (updateResult.IsFailure)
                return Result.Failure(updateResult.Error);

            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
