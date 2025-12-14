using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class JoinClub : BaseEntity
    {
        private JoinClub() : base(Guid.Empty)
        {
        }
        private JoinClub(Guid id, JoinClubRequestStatus joinClubRequestStatus, Guid userId, Guid clubId)
            : base(id)
        {
            JoinClubRequestStatus = joinClubRequestStatus;
            UserId = userId;
            ClubId = clubId;
        }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid ClubId { get; private set; }
        public JoinClubRequestStatus JoinClubRequestStatus { get; private set; }
        public Club Club { get; private set; }

        public static Result<JoinClub> Create(JoinClubRequestStatus joinClubRequestStatus, Guid userId, Guid clubId, ClubMemberRole clubMemberRole)
        {
            if(!Enum.IsDefined(typeof(JoinClubRequestStatus), joinClubRequestStatus)) return Result.Failure<JoinClub>(DomainErrors.JoinClub.InvalidStatus);

            var joinClub = new JoinClub(Guid.NewGuid(), joinClubRequestStatus, userId, clubId);

            return Result.Success(joinClub);
        }

        public Result Approve()
        {
            if (JoinClubRequestStatus != JoinClubRequestStatus.Pending) return Result.Failure(DomainErrors.JoinClub.NotPending);

            JoinClubRequestStatus = JoinClubRequestStatus.Approved;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }
        public Result Reject()
        {
            if (JoinClubRequestStatus != JoinClubRequestStatus.Pending) return Result.Failure(DomainErrors.JoinClub.NotPending);

            JoinClubRequestStatus = JoinClubRequestStatus.Rejected;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
