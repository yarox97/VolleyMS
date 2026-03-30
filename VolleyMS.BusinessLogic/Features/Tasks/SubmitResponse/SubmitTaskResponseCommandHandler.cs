using MediatR;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.SubmitResponse
{
    public sealed class SubmitTaskResponseCommandHandler : IRequestHandler<SubmitTaskResponseCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public SubmitTaskResponseCommandHandler(
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

        public async Task<Result> Handle(SubmitTaskResponseCommand command, CancellationToken cancellationToken)
        {
            // get user task
            var userTask = await _taskRepository.GetUserTaskByTaskIdAndUserIdAsync(
                command.TaskId,
                command.UserId,
                cancellationToken);

            if (userTask is null)
                return Result.Failure(DomainErrors.UserTask.UserTaskNotFound);


            // check permissions
            var user = await _userRepository.GetByIdAsync(command.UserId);
            if (user is null)
                return Result.Failure(DomainErrors.User.UserNotFound);

            if(userTask.Task.Receivers.Any(r => r.UserId == command.UserId) == false)
                return Result.Failure(DomainErrors.Club.InvalidPermission);

            // files attachment handling 
            var mediaFiles = new List<MediaFile>();

            if (command.Files != null && command.Files.Any())
            {
                var uploadTasks = command.Files.Select(async fileDto =>
                {
                    var url = await _fileService.UploadFileAsync(fileDto, "TaskResponses");

                    if (string.IsNullOrWhiteSpace(url)) return null;

                    var storageId = CloudinaryService.GetPublicIdFromUrl(url);

                    var mediaResult = MediaFile.Create(
                        url: url,
                        storageId: storageId,
                        fileName: fileDto.FileName,
                        contentType: fileDto.ContentType,
                        size: fileDto.Length
                    );

                    return mediaResult.IsSuccess ? mediaResult.Value : null;
                });

                var uploadedFiles = await System.Threading.Tasks.Task.WhenAll(uploadTasks);
                mediaFiles.AddRange(uploadedFiles.Where(f => f != null)!);
            }

            // add response to userTask
            var responseResult = userTask.AddUserResponse(mediaFiles, command.Text);

            if (responseResult.IsFailure)
                return Result.Failure(responseResult.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}