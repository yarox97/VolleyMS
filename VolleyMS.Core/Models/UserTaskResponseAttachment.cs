namespace VolleyMS.Core.Models
{
    public class UserTaskResponseAttachment
    {
        public UserTaskResponse UserTaskResponse { get; set; }
        public Guid UserTaskResponseId { get; set; }

        public MediaFile Media { get; set; }
        public Guid MediaId { get; set; }
    }
}
