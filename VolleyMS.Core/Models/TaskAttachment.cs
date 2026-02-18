using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class TaskAttachment : BaseEntity
    {
        private TaskAttachment() 
            : base(Guid.Empty)
        {
        }
        private TaskAttachment(Guid id, MediaFile MediaFile, Guid TaskId)
            : base(id)
        {
            this.MediaFile = MediaFile;
            this.TaskId = TaskId;
        }

        public static Result<TaskAttachment> Create(MediaFile MediaFile, Guid TaskId)
        {
            if (MediaFile == null) return Result.Failure<TaskAttachment>(
                new Error("MediaFile.NotFound",
                            "Specified mediafile could not be found")
                );

            if (TaskId == Guid.Empty) return Result.Failure<TaskAttachment>(
                DomainErrors.Task.TaskNotFound);

            return Result.Success(new TaskAttachment(Guid.NewGuid(), MediaFile, TaskId));
        }

        public Guid TaskId { get; private init; }
        public Task Task { get; private init; }
        public MediaFile MediaFile { get; private set; }
    }
}
