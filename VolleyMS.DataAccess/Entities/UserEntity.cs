using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities

{
    public class UserEntity : BaseEntity
    {
        public UserEntity() 
            : base(Guid.NewGuid())
        {
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; } = UserType.Player;
        public string Name { get; set; }
        public string Surname { get; set; }

        public IList<UserClubsEntity> UserClubs { get; set; }
        public IList<ContractEntity> Contracts { get; set; }
        public IList<TaskEntity> SentTasks { get; set; }
        public IList<TaskEntity> ReceivedTasks { get; set; }
        public IList<CommentEntity> SentComments { get; set; }
        public IList<NotificationEntity> SentNotifications { get; set; }
        public IList<UserNotificationsEntity> ReceivedNotifications { get; set; }
        public IList<JoinClubEntity> JoinClubRequests { get; set; }
    }
}
