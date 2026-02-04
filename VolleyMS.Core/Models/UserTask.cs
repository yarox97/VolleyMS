using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class UserTask : BaseEntity
    {
        private readonly List<TaskProof> _proofs = new();
        private readonly List<Comment> _comments = new();

        public UserTask() : base(Guid.Empty)
        {
        }

        public IReadOnlyCollection<TaskProof> Proofs => _proofs.AsReadOnly();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid TaskId { get; private set; }
        public Task Task { get; private set; }
        public TaskStatus Status { get; private set; }
        public string? ProofUrl { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        public static Result<UserTask> Create(User user, Task task)
        {
            return new UserTask
            {
                Id = Guid.NewGuid(),
                Task = task,
                TaskId = task.Id,
                User = user,
                UserId = user.Id,
                Status = TaskStatus.Uncompleted,
            };
        }
        public Result Complete()
        {
            CompletedAt = DateTime.UtcNow;
            Status = TaskStatus.Completed;

            return Result.Success();
        }

        public Result AddProof(string name, string url, ProofType type)
        {
            if (_proofs.Count >= 5)
                return Result.Failure(DomainErrors.Task.MaxProofsAllowed);

            var proofResult = TaskProof.Create(this.Id, name, url, type);

            if (proofResult.IsFailure)
                return Result.Failure(proofResult.Error);

            _proofs.Add(proofResult.Value);
            return Result.Success();
        }


        public Result RemoveProof(Guid proofId)
        {
            var proof = _proofs.FirstOrDefault(p => p.Id == proofId);
            if (proof == null)
                return Result.Failure(DomainErrors.Task.ProofNotFound);

            _proofs.Remove(proof);
            return Result.Success();
        }
    }
}
