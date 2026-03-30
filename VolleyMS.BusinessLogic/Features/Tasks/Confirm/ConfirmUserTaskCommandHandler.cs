using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Confirm
{
    public sealed class ConfirmUserTaskCommandHandler : IRequestHandler<ConfirmUserTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmUserTaskCommandHandler(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ConfirmUserTaskCommand command, CancellationToken cancellationToken)
        {
            // get user task
            var userTask = await _taskRepository.GetUserTaskByIdAsync(command.UserTaskId, cancellationToken);

            if (userTask is null)
                return Result.Failure(DomainErrors.UserTask.UserTaskNotFound);

            // check permissions
            var requestor = await _userRepository.GetByIdWithClubsAsync(command.RequestorId);
            if (requestor is null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            var requestorRoleResult = requestor.GetRoleInClub(userTask.Task.ClubId);
            if (requestorRoleResult.IsFailure || requestorRoleResult?.Value == null)
                return Result.Failure(DomainErrors.Club.MemberNotFound);

            if (!ClubRoleConfig.HasPermission(requestorRoleResult.Value, ClubPermission.EditTask))
                return Result.Failure(DomainErrors.Club.InvalidPermission);

            // confirm user task accomplishment
            var confirmResult = userTask.Confirm(command.FinalStatus, command.ApplyPenalty, requestor.Id);

            if (confirmResult.IsFailure)
                return Result.Failure(confirmResult.Error);

            // save changes (domain event makes payslip adjustment during saving changes)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}