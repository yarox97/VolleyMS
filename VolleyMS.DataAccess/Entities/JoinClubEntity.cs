using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities
{
    public class JoinClubEntity : BaseEntity
    {
        public JoinClubEntity()
            : base(Guid.NewGuid())
        {
        }
        public JoinClubRequestStatus requestStatus { get; set; }
        public Guid UserId { get; set; }
        public UserEntity UserEntity { get; set; } 
        public Guid ClubId { get; set; }
        public ClubEntity ClubEntity { get; set; }
    }
}
