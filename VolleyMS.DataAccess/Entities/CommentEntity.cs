using VolleyMS.Core.Common;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Models
{
    public class CommentEntity : BaseEntity
    {
        public CommentEntity()
        {
            Task = new TaskEntity();
            Sender = new UserEntity();
        }
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public Guid TaskId { get; set; }
        public TaskEntity Task;
        public Guid SenderId { get; set; }
        public UserEntity Sender { get; set; }
    }
}