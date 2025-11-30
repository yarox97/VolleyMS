using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities
{
    public class UserNotificationsEntity : BaseEntity
    {
        public UserNotificationsEntity() 
            : base(Guid.NewGuid())
        {
        }
        public Guid NotificationId { get; set; }
        public NotificationEntity Notification { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public bool IsChecked { get; set; } = false;
        public string? PayLoad { get; set; }
    }
}
