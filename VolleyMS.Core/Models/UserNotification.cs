using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models 
{ 
    public class UserNotification : BaseEntity
    {
        private UserNotification() : base(Guid.Empty)
        {
        }
        private UserNotification(Guid id, User user, Notification notification)
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

        public void Check()
        {
            if (!IsChecked)
            {
                IsChecked = true;
            }
        }

        public static Result<UserNotification> Create(User user, Notification notification)
        {
            if (user == null)
                return Result.Failure<UserNotification>(DomainErrors.User.UserNotFound);

            if (notification == null)
                return Result.Failure<UserNotification>(DomainErrors.Notification.NotificationNotFound);

            return Result.Success(new UserNotification(Guid.NewGuid(), user, notification));
        }
    }
}
