using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class Notification : BaseEntity
    {
        private readonly List<UserNotification> _userNotifications = new();
        private Notification() : base(Guid.Empty)
        {
            RequiredClubMemberRoles = new List<ClubMemberRole>();
        }
        private Notification(
            Guid id,
            IList<ClubMemberRole> requiredClubMemberRoles,
            NotificationCategory notificationCategory,
            string text,
            string? linkedUrl,
            string? payload,
            Guid? senderId)
            : base(id)
        {
            RequiredClubMemberRoles = requiredClubMemberRoles;
            Category = notificationCategory;
            Text = text;
            LinkedURL = linkedUrl;
            Payload = payload;
            SenderId = senderId;
        }
        public IList<ClubMemberRole> RequiredClubMemberRoles { get; private set; }
        public NotificationCategory Category { get; private set; }
        public string Text { get; private set; }
        public string? LinkedURL { get; private set; }
        public string? Payload { get; private set; }
        public Guid? SenderId { get; private set; }
        public User? Sender { get; private set; }

        public IReadOnlyCollection<UserNotification> UserNotifications => _userNotifications;

        public static Result<Notification> Create(
            IList<ClubMemberRole> requiredClubMemberRoles,
            NotificationCategory notificationCategory,
            string text,
            string? linkedUrl,
            string? payload,
            User? sender = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Result.Failure<Notification>(DomainErrors.Notification.TextEmpty);

            if (requiredClubMemberRoles == null || !requiredClubMemberRoles.Any())
                return Result.Failure<Notification>(DomainErrors.Notification.RolesEmpty);

            var notification = new Notification(
                Guid.NewGuid(),
                requiredClubMemberRoles,
                notificationCategory,
                text,
                linkedUrl,
                payload,
                sender?.Id
            );

            return Result.Success(notification);
        }
    }
}