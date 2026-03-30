using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.GetDetails
{
    public sealed class GetTaskDetailsQueryHandler : IRequestHandler<GetTaskDetailsQuery, Result<TaskDetailsDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;

        public GetTaskDetailsQueryHandler(ITaskRepository taskRepository, IUserRepository userRepository, IContractRepository contractRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _contractRepository = contractRepository;
        }

        public async Task<Result<TaskDetailsDto>> Handle(GetTaskDetailsQuery request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdWithDetailsAndResponses(request.TaskId, cancellationToken);

            if (task is null)
                return Result.Failure<TaskDetailsDto>(DomainErrors.Task.TaskNotFound);

            var user = await _userRepository.GetByIdWithClubsAsync(request.UserId);
            if (user is null)
                return Result.Failure<TaskDetailsDto>(DomainErrors.User.UserNotFound);

            var userRoleInClubResult = user.GetRoleInClub(task.ClubId);
            if (userRoleInClubResult.IsFailure || userRoleInClubResult?.Value == null)
                return Result.Failure<TaskDetailsDto>(DomainErrors.Club.MemberNotFound);

            bool hasViewAllPermission = ClubRoleConfig.HasPermission(userRoleInClubResult.Value, ClubPermission.ViewTasks);
            bool isReceiver = task.Receivers.Any(t => t.UserId == user.Id);

            if (!hasViewAllPermission && !isReceiver)
            {
                return Result.Failure<TaskDetailsDto>(DomainErrors.Club.InvalidPermission);
            }

            var visibleReceivers = hasViewAllPermission
                ? task.Receivers
                : task.Receivers.Where(r => r.UserId == user.Id).ToList();

            var receiverDtos = new List<TaskReceiverDto>();

            foreach (var r in visibleReceivers)
            {
                var receiverContract = await _contractRepository.GetByUserIdAndClubId(r.UserId, task.ClubId, cancellationToken);

                receiverDtos.Add(new TaskReceiverDto(
                    UserTaskId: r.Id,
                    UserId: r.UserId,
                    FullName: $"{r.User.Name} {r.User.Surname}",
                    TaskStatus: r.Status,
                    CompletedAt: r.CompletedAt,
                    Responses: MapAllResponses(r),
                    HasActiveContract: receiverContract != null && receiverContract.IsActive
                ));
            }

            var detailsDto = new TaskDetailsDto(
                Task: task.ToDto(request.UserId),
                Receivers: receiverDtos
            );

            return Result.Success(detailsDto);
        }

        private static List<UserTaskResponseDto> MapAllResponses(UserTask userTask)
        {
            if (userTask.TaskResponses == null || !userTask.TaskResponses.Any())
                return new List<UserTaskResponseDto>();

            return userTask.TaskResponses
                .OrderBy(r => r.CreatedAt)
                .Select(r => new UserTaskResponseDto(
                    Id: r.Id,
                    Text: r.Text,
                    CreatedAt: r.CreatedAt,
                    Attachments: r.Attachments?.Select(a => new AttachmentDto(
                        FileName: a.Media.FileName,
                        Url: a.Media.Url,
                        Size: a.Media.Size
                    )).ToList() ?? new List<AttachmentDto>()
                ))
                .ToList();
        }
    }
}