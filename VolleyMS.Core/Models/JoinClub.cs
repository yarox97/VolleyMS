using VolleyMS.Core.Common;

namespace VolleyMS.Core.Models
{
    public class JoinClub : BaseEntity
    {
        private JoinClub(JoinClubRequestStatus joinClubRequestStatus, Guid userId, Guid clubId)
            : base(Guid.NewGuid())
        {
            JoinClubRequestStatus = joinClubRequestStatus;
            UserId = userId;
            ClubId = clubId;
        }
        public Guid UserId { get; }
        public Guid ClubId { get; }
        public JoinClubRequestStatus JoinClubRequestStatus { get; }

        public static JoinClub Create(JoinClubRequestStatus joinClubRequestStatus, Guid userId, Guid clubId)
        {
            if(!Enum.IsDefined(typeof(JoinClubRequestStatus), joinClubRequestStatus))
            {
                throw new ArgumentException("Invalid join club request status", nameof(joinClubRequestStatus)); // TO CUSTOM EXCEPTION!
            }
            return new JoinClub(joinClubRequestStatus, userId, clubId);
        }
    }
}
