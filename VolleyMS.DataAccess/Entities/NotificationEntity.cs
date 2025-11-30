using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities
{
    public class NotificationEntity : BaseEntity
    {
        public NotificationEntity() 
            : base(Guid.NewGuid())
        {
        }
        public string Text { get; set;  }
        public string? LinkedURL { get; set; }
        public string? Payload { get; set; }
        public Guid? SenderId { get; set; }
        public UserEntity? Sender { get; set; }
        public NotificationCategory Category { get; set; }
        public IList<ClubMemberRole> RequiredClubMemberRoles { get; set; }
        public ICollection<UserNotificationsEntity> UserNotifications { get; set; }
    }
}
