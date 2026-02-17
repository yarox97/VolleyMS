using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.UpdateReceivers
{
    public class UpdateTaskReceiversCommandHandler : IRequestHandler<UpdateTaskReceiversCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository; 
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskReceiversCommandHandler(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateTaskReceiversCommand command, CancellationToken ct)
        {
            var task = await _taskRepository.GetByIdWithReceiversAsync(command.TaskId);

            if (task is null)
                return Result.Failure(DomainErrors.Task.TaskNotFound);

            var newReceivers = await _userRepository.GetUsersByIdsAsync(command.NewReceiverIds, ct);

            if (!newReceivers.Any() && command.NewReceiverIds.Any())
                return Result.Failure(DomainErrors.User.UserNotFound);

            var updateResult = task.UpdateReceivers(newReceivers);

            if (updateResult.IsFailure)
                return Result.Failure(updateResult.Error);

            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
