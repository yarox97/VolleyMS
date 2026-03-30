using MediatR;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;
using VolleyMS.DataAccess.Repositories;

namespace VolleyMS.BusinessLogic.Features.Tasks.AddAttachment
{
    public class AddTaskAttachmentCommandHandler : IRequestHandler<AddTaskAttachmentCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public AddTaskAttachmentCommandHandler(
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

        public async Task<Result> Handle(AddTaskAttachmentCommand command, CancellationToken ct)
        {
            var uploadedPublicIds = new List<string>();
            var mediaFiles = new List<MediaFile>();

            try
            {
                foreach (var formFile in command.Files)
                {
                    var url = await _fileService.UploadFileAsync(formFile, "TaskAttachments");
                    if (string.IsNullOrEmpty(url)) continue;

                    var storageId = CloudinaryService.GetPublicIdFromUrl(url);
                    uploadedPublicIds.Add(storageId);

                    var mediaFileResult = MediaFile.Create(
                        url, storageId, formFile.FileName, formFile.ContentType, formFile.Length);

                    if (mediaFileResult.IsSuccess)
                        mediaFiles.Add(mediaFileResult.Value);
                }

                var task = await _taskRepository.GetWithAttachments(command.TaskId);
                if (task is null)
                    return Result.Failure(DomainErrors.Task.TaskNotFound);

                // check permissions
                var user = await _userRepository.GetByIdWithClubsAsync(command.RequestorId);
                if (user is null)
                    return Result.Failure(DomainErrors.User.UserNotFound);

                var userRoleResult = user.GetRoleInClub(task.ClubId);
                if (userRoleResult.IsFailure || userRoleResult?.Value == null)
                    return Result.Failure(DomainErrors.Club.MemberNotFound);

                if (!ClubRoleConfig.HasPermission(userRoleResult.Value, ClubPermission.EditTask))
                    return Result.Failure(DomainErrors.Club.InvalidPermission);


                if (command.Files == null || !command.Files.Any())
                    return Result.Success();

                var result = task.AddAttachments(mediaFiles);

                if (result.IsFailure)
                    return Result.Failure(result.Error);

                await _unitOfWork.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception)
            {
                foreach (var publicId in uploadedPublicIds)
                {
                    await _fileService.DeleteFileAsync(publicId);
                }
                return Result.Failure(new Error("FileServie.Error", "File service returned error while adding task attachment"));
            }
        }
    }
}
