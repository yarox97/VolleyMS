using VolleyMS.Core.Common;
using VolleyMS.Core.DomainEvents.Tasks;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class Task : BaseEntity
    {
        private readonly List<UserTask> _receivers = new();
        private readonly List<UserTaskResponse> _responses = new();
        private readonly List<TaskAttachment> _attachments = new();

        private Task() : base(Guid.Empty)
        {
        }

        private Task(
            Guid id,
            string title,
            string description,
            TaskType taskType,
            TaskPriority taskPriority,
            PenaltyType penaltyType,
            decimal? penaltyAmount,
            TaskSchedule taskSchedule,
            string? payload,
            Club club,
            User sender)
            : base(id)
        {
            Title = title;
            Description = description;
            TaskType = taskType;
            TaskPriority = taskPriority;
            PenaltyType = penaltyType;
            PenaltyAmount = penaltyAmount;
            TaskSchedule = taskSchedule;
            Payload = payload;

            Club = club;
            ClubId = club.Id;

            Sender = sender;
            SenderId = sender.Id;
        }

        public TaskType TaskType { get; private set; }
        public TaskPriority TaskPriority { get; private set; }
        public PenaltyType PenaltyType { get; private set; }
        public decimal? PenaltyAmount { get; private set; }
        public TaskSchedule TaskSchedule { get; private set; }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string? Payload { get; private set; }

        public Guid SenderId { get; private set; }
        public User Sender { get; private set; }

        public Guid ClubId { get; private set; }
        public Club Club { get; private set; }

        public IReadOnlyCollection<UserTask> Receivers => _receivers;
        public IReadOnlyCollection<UserTaskResponse> Responses => _responses;
        public IReadOnlyCollection<TaskAttachment> Attachments => _attachments;

        public static Result<Task> Create(
            string title,
            string description,
            TaskSchedule taskSchedule,
            TaskType taskType,
            TaskPriority taskPriority,
            PenaltyType penaltyType,
            decimal? penaltyAmount,
            string? payload,
            Club club,
            User sender)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Task>(DomainErrors.Task.TitleEmpty);

            if (club is null)
                return Result.Failure<Task>(DomainErrors.Club.ClubNotFound);

            if (sender is null)
                return Result.Failure<Task>(DomainErrors.User.UserNotFound);

            var task = new Task(
                Guid.NewGuid(),
                title,
                description,
                taskType,
                taskPriority,
                penaltyType,
                penaltyAmount,
                taskSchedule,
                payload,
                club,
                sender
            );

            return Result.Success(task);
        }

        public Result UpdateDetails(
            string title,
            string description,
            TaskPriority taskPriority,
            PenaltyType penaltyType,
            decimal? penaltyAmount,
            TaskSchedule taskSchedule,
            string? payload)
        {
            if (DateTime.UtcNow >= TaskSchedule.StartDate)
            {
                if (PenaltyType != penaltyType || PenaltyAmount != penaltyAmount)
                {
                    return Result.Failure(new Error(
                        "Task.CannotChangePenalty",
                        "Cannot change penalty of already started task!."));
                }
            }

            var domainEvent = new TaskUpdatedDomainEvent(
            TaskId: this.Id,
            OldTitle: this.Title,
            OldDescription: this.Description,
            OldTaskPriority: this.TaskPriority,
            OldStartDate: this.TaskSchedule.StartDate,
            OldEndDate: (DateTime)this.TaskSchedule.EndDate
            );

            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure(DomainErrors.Task.TitleEmpty);

            Title = title;
            Description = description;
            TaskPriority = taskPriority;
            PenaltyType = penaltyType;
            PenaltyAmount = penaltyAmount;
            TaskSchedule = taskSchedule;
            Payload = payload;

            RaiseDomainEvent(domainEvent);
            return Result.Success();
        }

        public Result UpdateReceivers(IEnumerable<User> newReceivers)
        {
            if (newReceivers == null)
                return Result.Failure(DomainErrors.Task.NoReceivers);

            var newUsersList = newReceivers.ToList();
            var newUserIds = newUsersList.Select(u => u.Id).ToHashSet();

            var toRemove = _receivers.Where(ut => !newUserIds.Contains(ut.UserId)).ToList();

            foreach (var userTask in toRemove)
            {
                _receivers.Remove(userTask);
            }

            var currentIds = _receivers.Select(ut => ut.UserId).ToHashSet();

            foreach (var user in newUsersList)
            {
                if (!currentIds.Contains(user.Id))
                {
                    var assignResult = UserTask.Create(user, this);
                    if (assignResult.IsFailure) return Result.Failure(assignResult.Error);

                    _receivers.Add(assignResult.Value);
                    RaiseDomainEvent(new TaskAssignedDomainEvent(this.Id, new[] { user }, ClubId));
                }
            }

            return Result.Success();
        }

        public Result AddAttachments(ICollection<MediaFile> mediaFiles)
        {
            if (mediaFiles == null) return Result.Success();

            foreach (var mediaFile in mediaFiles)
            {
                if (_attachments.Any(a => a.MediaFile.Id == mediaFile.Id))
                    continue;

                var attachmentResult = TaskAttachment.Create(mediaFile, this.Id);
                if (attachmentResult.IsSuccess)
                {
                    _attachments.Add(attachmentResult.Value);
                }
            }
            return Result.Success();
        }

        public Result RemoveAttachment(MediaFile mediaFile)
        {
            if (mediaFile == null) return Result.Failure(new Error("File.Null", "File is null"));

            var attachment = _attachments.FirstOrDefault(a => a.MediaFile.Id == mediaFile.Id);

            if (attachment == null)
            {
                return Result.Failure(new Error("Attachment.NotFound", "This file is not attached to the task"));
            }

            _attachments.Remove(attachment);
            return Result.Success();
        }

        public Result AssignTask(IEnumerable<User> receivers)
        {
            if (receivers == null || !receivers.Any())
                return Result.Failure(DomainErrors.Task.NoReceivers);

            foreach (var user in receivers)
            {
                var assignResult = UserTask.Create(user, this);
                if (assignResult.IsFailure) return Result.Failure(assignResult.Error);

                _receivers.Add(assignResult.Value);
            }

            RaiseDomainEvent(new TaskAssignedDomainEvent(this.Id, receivers, ClubId));

            return Result.Success();
        }
    }
}