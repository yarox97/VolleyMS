using VolleyMS.Core.Common;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class TaskProof : BaseEntity
    {
        public Guid UserTaskId { get; private set; }

        public string Url { get; private set; }
        public string Name { get; private set; }
        public ProofType Type { get; private set; }

        private TaskProof(Guid id, Guid userTaskId, string name, string url, ProofType type)
            : base(id)
        {
            UserTaskId = userTaskId;
            Url = url;
            Type = type;
        }

        public static Result<TaskProof> Create(Guid userTaskId, string name, string url, ProofType type)
        {
            if (string.IsNullOrWhiteSpace(url))
                return Result.Failure<TaskProof>(Error.NullValue);

            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<TaskProof>(Error.NullValue);

            return Result.Success(new TaskProof(Guid.NewGuid(), userTaskId, name, url, type));
        }
    }
}