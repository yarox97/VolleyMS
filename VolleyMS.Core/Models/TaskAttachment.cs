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
        private TaskAttachment(Guid id, MediaFile MediaFile, Task Task)
            : base(id)
        {
            this.MediaFile = MediaFile;
            TaskId = Task.Id;
            this.Task = Task;
        }

        public static Result<TaskAttachment> Create(MediaFile MediaFile, Task Task)
        {
            if (MediaFile == null) return Result.Failure<TaskAttachment>(
                new Error("MediaFile.NotFound",
                            "Specified mediafile could not be found")
                );

            if (Task == null) return Result.Failure<TaskAttachment>(
                DomainErrors.Task.TaskNotFound);

            return Result.Success(new TaskAttachment(Guid.NewGuid(), MediaFile, Task));
        }

        public Guid TaskId { get; private init; }
        public Task Task { get; private init; }
        public MediaFile MediaFile { get; private set; }
    }
}
