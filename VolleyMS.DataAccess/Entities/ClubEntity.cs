using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities
{
    public class ClubEntity : BaseEntity
    {
        public ClubEntity() : base(Guid.NewGuid())
        {
        }
        public string Name { get; set; }
        public string JoinCode { get; set; }
        public string? Description { get; set; }
        public string? AvatarURL { get; set; }
        public string? BackGroundURL { get; set;  } 
        public IList<TaskEntity> Tasks { get;  set; }
        public IList<UserClubsEntity> UserClubs { get; set; }
        public IList<JoinClubEntity> JoinClubRequests { get; set; }
    }
}
