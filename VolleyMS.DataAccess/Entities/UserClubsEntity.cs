using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities
{
    public class UserClubsEntity : BaseEntity
    {
        public UserClubsEntity() : base(Guid.NewGuid())
        {
        }
        public UserEntity User { get; set; }
        public Guid UserId { get; set; }
        public ClubEntity Club { get; set; }
        public Guid ClubId { get; set; }
        public IList<ClubMemberRole> ClubMemberRole { get; set; }
    }
}
