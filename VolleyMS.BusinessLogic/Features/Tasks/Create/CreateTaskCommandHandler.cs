using MediatR;
using VolleyMS.BusinessLogic.Features.Notifications.NotificationPayloads;
using VolleyMS.BusinessLogic.Features.Tasks.TaskPayloads;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Tasks.Create
{

    public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<Core.Models.Task>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClubRepository _clubRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskCommandHandler(IUserRepository userRepository, IClubRepository clubRepository, ITaskRepository taskRepository, IFileService fileService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
            _clubRepository = clubRepository;
            _fileService = fileService;
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

            var taskRequest = command.TaskRequest;

            // Check club existence
            var club = await _clubRepository.GetByIdWithMembersAsync(command.TaskRequest.ClubId, cancellationToken);
            if(club is null)
                return Result.Failure<Core.Models.Task>(DomainErrors.Club.ClubNotFound);

            // Check sender existence
            var sender = await _userRepository.GetByIdAsync(command.UserId);
            if (sender is null)
                return Result.Failure<Core.Models.Task>(DomainErrors.User.UserNotFound);

            // Check if the receivers are members of the club
            if (command.TaskRequest.Receivers == null || !command.TaskRequest.Receivers.Any())
                return Result.Failure<Core.Models.Task>(DomainErrors.Task.NoReceivers);

            var validReceiversIds = new List<Guid>();
            foreach(Guid ReceiverId in command.TaskRequest.Receivers)
            {
                if (club.HasMember(ReceiverId))
                    validReceiversIds.Add(ReceiverId);
            }

            // create task schedule if days of week are provided
            var taskSchedule = default(Core.Models.TaskSchedule);

            if (taskRequest.DaysOfWeek != null && taskRequest.DaysOfWeek.Any())
            {
                // create task schedule fo the recurring task
                var taskScheduleResult = TaskSchedule.CreateReccuring(
                    startDate: taskRequest.StartDate, 
                    endDate: taskRequest.EndDate, 
                    daysOfWeek: taskRequest.DaysOfWeek,
                    startTime: taskRequest.StartTime,
                    endTime: taskRequest.EndTime);

                if (taskScheduleResult.IsFailure)
                    return Result.Failure<Core.Models.Task>(taskScheduleResult.Error);

                taskSchedule = taskScheduleResult.Value;
            }
            else
            {
                // create task schedule for the one-time task
                var taskScheduleResult = TaskSchedule.CreateOneTime(
                    date: taskRequest.StartDate, 
                    startTime: taskRequest.StartTime,
                    endTime: taskRequest.EndTime);

                if (taskScheduleResult.IsFailure)
                    return Result.Failure<Core.Models.Task>(taskScheduleResult.Error);

                taskSchedule = taskScheduleResult.Value;
            }

            // create task payload
            var taskPayload = TaksPayloadFactory.CreatePayLoad(taskRequest, command.UserId);
            if(taskPayload.IsFailure)
                return Result.Failure<Core.Models.Task>(taskPayload.Error);

            var payload = taskPayload.Value;

            // create task
            var taskCreateResult = Core.Models.Task.Create(
                title: taskRequest.Title,
                description: taskRequest.Description,
                taskSchedule: taskSchedule,
                taskType: taskRequest.TaskType,
                penaltyType: taskRequest.penaltyType,
                payload: payload,
                club: club,
                sender: sender
                );

            if(taskCreateResult.IsFailure)
                return Result.Failure<Core.Models.Task>(taskCreateResult.Error);

            var taskValue = taskCreateResult.Value;


            // Add attachments if exist
            if (taskRequest.Files != null && taskRequest.Files.Any())
            {
                var mediaFiles = new List<MediaFile>();

                foreach (var formFile in taskRequest.Files)
                {
                    var fileUrl = await _fileService.UploadFileAsync(formFile, "TaskAttachments");

                    if (string.IsNullOrWhiteSpace(fileUrl))
                        continue;

                    var storageId = CloudinaryService.GetPublicIdFromUrl(fileUrl);

                    var mediaFileResult = MediaFile.Create(
                        url: fileUrl,
                        storageId: storageId,
                        fileName: formFile.FileName,
                        contentType: formFile.ContentType,
                        size: formFile.Length
                    );

                    if (mediaFileResult.IsSuccess)
                    {
                        mediaFiles.Add(mediaFileResult.Value);
                    }
                }

                if (mediaFiles.Any())
                {
                    var attachResult = taskValue.AddAttachments(mediaFiles);
                    if (attachResult.IsFailure)
                        return Result.Failure<Core.Models.Task>(attachResult.Error);
                }
            }


            // assign task to receivers
            var users = await _userRepository.GetUsersByIdsAsync(validReceiversIds);
            if (users.Any())
            {
                taskValue.AssignTask(users);
            }
            else
            {
                return Result.Failure<Core.Models.Task>(DomainErrors.Task.NoReceivers);
            }

            await _taskRepository.AddAsync(taskValue);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(taskValue);
        }
    }
}
