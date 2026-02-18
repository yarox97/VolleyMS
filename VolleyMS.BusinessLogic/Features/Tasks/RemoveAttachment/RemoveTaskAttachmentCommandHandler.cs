using MediatR;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.RemoveAttachment
{
    public class RemoveTaskAttachmentCommandHandler : IRequestHandler<RemoveTaskAttachmentCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveTaskAttachmentCommandHandler(
            ITaskRepository taskRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveTaskAttachmentCommand command, CancellationToken ct)
        {


            var task = await _taskRepository.GetWithAttachments(command.TaskId);
            if (task is null)
                return Result.Failure(DomainErrors.Task.TaskNotFound);

            var attachment = task.Attachments.FirstOrDefault(a => a.MediaFile.Id == command.MediaFileId);
            if (attachment == null)
                return Result.Failure(new Error("Attachment.NotFound", "File not found in task"));

            var storageId = attachment.MediaFile.StorageId;

            var removeResult = task.RemoveAttachment(attachment.MediaFile);
            if (removeResult.IsFailure)
                return Result.Failure(removeResult.Error);

            await _unitOfWork.SaveChangesAsync(ct);

            if (!string.IsNullOrEmpty(storageId))
            {
                await _fileService.DeleteFileAsync(storageId);
            }

            return Result.Success();
        }
    }
}
