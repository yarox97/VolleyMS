using VolleyMS.Core.Common;

namespace VolleyMS.Core.Models
{
    public class Comment : BaseEntity
    {
        public Comment(Guid id)
            : base(id)
        {
        }
        public string Text { get; private set; }
        public Guid TaskId { get; private set; }
        public Task Task { get; private set; }
        public Guid SenderId { get; private set; }
        public User Sender { get; private set; }
    }
}
