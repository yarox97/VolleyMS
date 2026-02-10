using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class TaskAttachment
    {
        private TaskAttachment(MediaFile MediaFile, Task Task)
        {
            MediaId = MediaFile.Id;
            Media = MediaFile;
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
                DomainErrors.Task.NotFound);

            return Result.Success(new TaskAttachment(MediaFile, Task));
        }

        public Guid TaskId { get; private init; }
        public Task Task { get; private init; }
        public Guid MediaId { get; private set; }
        public MediaFile Media { get; private set; }
    }
}
