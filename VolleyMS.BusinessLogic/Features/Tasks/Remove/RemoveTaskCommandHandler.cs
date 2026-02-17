using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Remove
{
    public class RemoveTaskCommandHandler : IRequestHandler<RemoveTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService; 
        private readonly IUnitOfWork _unitOfWork;

        public RemoveTaskCommandHandler(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveTaskCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdWithClubsAsync(command.UserId);
            if (user == null)
            {
                return Result.Failure<PaginatedResult<TaskDto>>(DomainErrors.User.UserNotFound);
            }

            var userRoleResult = user.GetRoleInClub(command.ClubId);
            if (userRoleResult.IsFailure || userRoleResult?.Value == null)
            {
                return Result.Failure<PaginatedResult<TaskDto>>(DomainErrors.Club.MemberNotFound);
            }
            var userRole = userRoleResult.Value;

            if (!ClubRoleConfig.HasPermission(userRole, ClubPermission.DeleteTask))
            {
                return Result.Failure<PaginatedResult<TaskDto>>(DomainErrors.Club.InvalidPermission);
            }

            var task = await _taskRepository.GetWithAttachments(command.TaskId);

            if (task is null)
                return Result.Failure(DomainErrors.Task.TaskNotFound);

            var filesToDelete = task.Attachments
                .Select(a => a.MediaFile.StorageId)
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            _taskRepository?.DeleteAsync(task.Id);

            await _unitOfWork.SaveChangesAsync(ct);

            foreach (var storageId in filesToDelete)
            {
                try { await _fileService.DeleteFileAsync(storageId); }
                catch { /* LOGG */}
            }

            return Result.Success();
        }
    }
}
