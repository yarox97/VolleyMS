using VolleyMS.Core.Common;
using VolleyMS.Core.Exceptions;

namespace VolleyMS.Core.Models
{
    public class Notification : BaseEntity
    {
        private Notification() : base(Guid.Empty)
        {
        }
        private Notification(Guid id, IList<ClubMemberRole> requiredClubMemberRoles, NotificationCategory notificationCategory, string text, string? linkedURL, string? payload)
            : base(id)
        {
            RequiredClubMemberRoles = requiredClubMemberRoles;
            Category = notificationCategory;
            Text = text;
            LinkedURL = linkedURL;
            Payload = payload;
        }
        public IList<ClubMemberRole> RequiredClubMemberRoles { get; }
        public NotificationCategory Category { get; } = NotificationCategory.Informative;
        public string Text { get; private set; }
        public string? LinkedURL { get; private set; } 
        public string? Payload { get; private set; }
        public User? Sender { get; private set; }
        public Guid? SenderId { get; private set; }
        public ICollection<UserNotification> UserNotifications { get; set; }

        public static Notification Create(IList<ClubMemberRole> requiredClubMemberRoles, NotificationCategory notificationCategory, string text, string? lindkedURL, string? payload)
        {
            if(string.IsNullOrEmpty(text))
            {
                throw new EmptyFieldDomainException("Notification text cannot be empty!");    
            }
            if(requiredClubMemberRoles == null)
            {
                throw new InvalidNotificationTypeDomainException("Notification type cannot be null!");
            }

            var notification = new Notification(Guid.NewGuid(), requiredClubMemberRoles, notificationCategory, text, lindkedURL, payload);
            return notification;
        }

        public static Notification Create(Guid id, IList<ClubMemberRole> requiredClubMemberRoles, NotificationCategory notificationCategory, string text, string? lindkedURL, string? payload)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new EmptyFieldDomainException("Notification text cannot be empty!");
            }
            if (requiredClubMemberRoles == null)
            {
                throw new InvalidNotificationTypeDomainException("Notification type cannot be null!");
            }

            var notification = new Notification(id, requiredClubMemberRoles, notificationCategory, text, lindkedURL, payload);
            return notification;
        }


    }
}
