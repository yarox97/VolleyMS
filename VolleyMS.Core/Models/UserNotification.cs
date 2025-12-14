using VolleyMS.Core.Common;

namespace VolleyMS.Core.Models 
{ 
    public class UserNotification : BaseEntity
    {
        private UserNotification() : base(Guid.Empty)
        {
        }
        public UserNotification(Guid id, User user, Notification notification)
            : base(id)
        {
            Notification = notification;
            NotificationId = notification.Id;
            User = user;
            UserId = user.Id;
            IsChecked = false;
        }
        public Guid NotificationId { get; }
        public Notification Notification { get; }
        public Guid UserId { get; }
        public User User { get; }
        public bool IsChecked { get; private set; }
        public string? PayLoad { get; }

        internal void Check()
        {
            if (!IsChecked)
            {
                IsChecked = true;
            }
        }
    }
}
