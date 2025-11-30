using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities
{
    public class CommentEntity : BaseEntity
    {
        public CommentEntity() : base(Guid.NewGuid())
        {
        }
        public string Text { get; set; }
        public Guid TaskId { get; set; }
        public TaskEntity Task;
        public Guid SenderId { get; set; }
        public UserEntity Sender { get; set; }
    }
}