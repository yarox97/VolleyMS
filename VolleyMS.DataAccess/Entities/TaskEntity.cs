using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities
{
    public class TaskEntity : BaseEntity
    {
        public TaskEntity() : base(Guid.NewGuid())
        {
        }
        public TaskType TaskType { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public PenaltyType PenaltyType { get; set; }
        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IList<CommentEntity> Comments { get; set; }
        public Guid SenderId { get; set; }
        public UserEntity Sender { get; set; }
        public IList<UserEntity> Receivers { get; set; }
        public Guid ClubId { get; set; }
        public ClubEntity Club { get; set; }
    }
}
