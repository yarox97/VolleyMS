namespace VolleyMS.Core.Models
{
    public class UserTaskResponseAttachment
    {
        public Guid ResponseId { get; set; }
        public UserTaskResponse UserTaskResponse { get; set; }

        public Guid MediaId { get; set; }
        public MediaFile Media { get; set; }
    }
}
