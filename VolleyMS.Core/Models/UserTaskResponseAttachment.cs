using VolleyMS.Core.Common;

namespace VolleyMS.Core.Models
{
    public class UserTaskResponseAttachment : BaseEntity
    {
        private UserTaskResponseAttachment() 
            : base(Guid.Empty)
        {
        }
        public UserTaskResponseAttachment(UserTaskResponse response, MediaFile mediaFile) 
            : base(Guid.NewGuid())
        {
            UserTaskResponse = response;
            UserTaskResponseId = response.Id;
            Media = mediaFile;
            MediaId = mediaFile.Id;
        }
        public UserTaskResponse UserTaskResponse { get; set; }
        public Guid UserTaskResponseId { get; set; }

        public MediaFile Media { get; set; }
        public Guid MediaId { get; set; }
    }
}
