using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Complete
{
    public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteTaskCommandHandler(ITaskRepository taskRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        {
            // get user task
            var userTask = await _taskRepository.GetUserTaskByIdAsync(request.UserTaskId, cancellationToken);
            if(userTask == null)
            {
                return Result.Failure(DomainErrors.UserTask.UserTaskNotFound);
            }

            // get user
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if(user == null)
            {
                return Result.Failure(DomainErrors.User.UserNotFound);
            }

            // check permissions
            if (userTask.UserId != user.Id)
            {
                return Result.Failure(DomainErrors.Club.InvalidPermission);
            }

            // complete task
            var result = userTask.Complete();
            if(result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
