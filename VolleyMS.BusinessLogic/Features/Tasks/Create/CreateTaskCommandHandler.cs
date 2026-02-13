using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Create
{

    public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<Core.Models.Task>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskCommandHandler(IUserRepository userRepository, IClubRepository clubRepository, ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
            _clubRepository = clubRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Core.Models.Task>> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            // Check task request existance
            if (command.TaskRequest == null)
                return Result.Failure<Core.Models.Task>(new Error
                    ("TaskRequest.NullValue", 
                    "Task request is null")
                    );

            // Check club existence
            var club = await _clubRepository.GetByIdWithMembersAsync(command.TaskRequest.ClubId, cancellationToken);
            if(club is null)
                return Result.Failure<Core.Models.Task>(DomainErrors.Club.ClubNotFound);

            // Check if the receivers are members of the club
            if (command.TaskRequest.Receivers == null || !command.TaskRequest.Receivers.Any())
                return Result.Failure<Core.Models.Task>(DomainErrors.Task.NoReceivers);

            var validReceiversIds = new List<Guid>();
            foreach(Guid ReceiverId in command.TaskRequest.Receivers)
            {
                if (club.HasMember(ReceiverId))
                    validReceiversIds.Add(ReceiverId);
            }




        }
    }
}
