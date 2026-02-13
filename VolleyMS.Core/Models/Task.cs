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
            PenaltyType penaltyType,
            TaskSchedule taskSchedule,
            string? payload,
            Club club,
            User sender)
            : base(id)
        {
            TaskType = taskType;
            PenaltyType = penaltyType;
            TaskSchedule = taskSchedule;
            Title = title;
            Description = description;
            Payload = payload;
            Club = club;
            ClubId = club.Id;
            Sender = sender;
            SenderId = sender.Id;
        }

        public TaskType TaskType { get; private set; }
        public PenaltyType PenaltyType { get; private set; }
        public TaskSchedule TaskSchedule { get; private set; }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string? Payload { get; private set; } // Json payload to a specific type of task.
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
            PenaltyType penaltyType,
            string? payload,
            Club club,
            User sender)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Task>(DomainErrors.Task.TitleEmpty);

            if(club is null)
                return Result.Failure<Task>(DomainErrors.Club.ClubNotFound);

            if(sender is null)
                return Result.Failure<Task>(DomainErrors.User.UserNotFound);

            var task = new Task(
                Guid.NewGuid(),
                title,
                description,
                taskType,
                penaltyType,
                taskSchedule,
                payload,
                club,
                sender
            );

            return Result.Success(task);
        }

        public Result AddAttachments(ICollection<MediaFile> mediaFiles)
        {
            var validMediaFiles = new List<MediaFile>();
            foreach (MediaFile mediaFile in mediaFiles)
            {
                if (mediaFile is not null)
                    validMediaFiles.Add(mediaFile);
            }

            foreach (MediaFile mediaFile in validMediaFiles)
            {
                var attachment = TaskAttachment.Create(mediaFile, this);
                if (attachment.IsFailure)
                    continue; // LOGG this but let the other files to be.
                    //return Result.Failure(attachment.Error);

                _attachments.Add(attachment.Value);
            }
            return Result.Success();
        }

        public Result AssignTask(IEnumerable<User> Receivers)
        {
            if (Receivers == null || !Receivers.Any())
                return Result.Failure(DomainErrors.Task.NoReceivers);

            foreach(var Receiver in Receivers)
            {
                var assignTaskResult = UserTask.Create(Receiver, this);

                if (assignTaskResult.IsFailure)
                    return Result.Failure(assignTaskResult.Error);

                _receivers.Add(assignTaskResult.Value); // Add receiver 
            }

            RaiseDomainEvent(new TaskAssignedDomainEvent(Receivers, ClubId));

            return Result.Success();
        }
    }
}