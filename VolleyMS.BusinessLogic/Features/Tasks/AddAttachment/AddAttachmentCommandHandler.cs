using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.AddAttachment
{
    public class AddTaskAttachmentCommandHandler : IRequestHandler<AddTaskAttachmentCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public AddTaskAttachmentCommandHandler(
            ITaskRepository taskRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddTaskAttachmentCommand command, CancellationToken ct)
        {
            var task = await _taskRepository.GetWithAttachments(command.TaskId);
            if (task is null)
                return Result.Failure(DomainErrors.Task.TaskNotFound);

            if (command.Files == null || !command.Files.Any())
                return Result.Success();

            var uploadedPublicIds = new List<string>();

            try
            {
                var mediaFiles = new List<MediaFile>();

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

                var result = task.AddAttachments(mediaFiles);

                if (result.IsFailure)
                    return Result.Failure(result.Error);

                await _unitOfWork.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (Exception)
            {
                foreach (var publicId in uploadedPublicIds)
                {
                    await _fileService.DeleteFileAsync(publicId);
                }
                return Result.Failure(new Error("Attachment.UploadFailed", "Failed to save attachments"));
            }
        }
    }
}
