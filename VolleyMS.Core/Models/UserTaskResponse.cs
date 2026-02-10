namespace VolleyMS.Core.Models
{
    public class UserTaskResponse
    {
        public Guid Id { get; private init; }

        public ICollection<UserTaskResponseAttachment> Attachments { get; private set; } = new List<UserTaskResponseAttachment>();
    }
}
