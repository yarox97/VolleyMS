using MediatR;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Return
{
    public class ReturnTaskCommandHandler : IRequestHandler<ReturnTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReturnTaskCommandHandler(ITaskRepository taskRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ReturnTaskCommand command, CancellationToken cancellationToken)
        {
            // get user task by id
            var userTask = await _taskRepository.GetUserTaskByIdAsync(command.userTaskId, cancellationToken);
            if (userTask is null)
                return Result.Failure(DomainErrors.UserTask.UserTaskNotFound);

            // check permission 
            var requestor = await _userRepository.GetByIdWithClubsAsync(command.userId);
            if (requestor is null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            var requestorRoleResult = requestor.GetRoleInClub(userTask.Task.ClubId);
            if (requestorRoleResult.IsFailure || requestorRoleResult?.Value == null)
                return Result.Failure(DomainErrors.Club.MemberNotFound);

            if (!ClubRoleConfig.HasPermission(requestorRoleResult.Value, ClubPermission.EditTask))
            {
                return Result.Failure(DomainErrors.Club.InvalidPermission);
            }

            // return task 
            var returnResult = userTask.Return();
            if (returnResult.IsFailure)
                return Result.Failure(returnResult.Error);

            // save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
