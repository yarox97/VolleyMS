using VolleyMS.Core.Common;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Models;

namespace VolleyMS.DataAccess.Entities

{
    public class UserEntity : BaseEntity
    {
        public UserEntity()
        {
            Contracts = new List<ContractEntity>();
            Clubs = new List<ClubEntity>();
            SentTasks = new List<TaskEntity>();
            ReceivedTasks = new List<TaskEntity>();
            SentComments = new List<CommentEntity>();
            ReceivedNotifications = new List<NotificationEntity>();
            SentNotifications = new List<NotificationEntity>();
        }
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType userType { get; set; } = UserType.Player;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public IList<ContractEntity> Contracts { get; set; }
        public IList<ClubEntity> Clubs { get; set; }
        public IList<TaskEntity> SentTasks { get; set; }
        public IList<TaskEntity> ReceivedTasks { get; set; }
        public IList<CommentEntity> SentComments { get; set; }
        public IList<NotificationEntity> SentNotifications { get;  set; }
        public IList<NotificationEntity> ReceivedNotifications { get; set; }
    }
}
