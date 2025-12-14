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
        private JoinClub(Guid id, Guid userId, Guid clubId)
            : base(id)
        {
            JoinClubRequestStatus = JoinClubRequestStatus.Pending;
            UserId = userId;
            ClubId = clubId;
        }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid ClubId { get; private set; }
        public JoinClubRequestStatus JoinClubRequestStatus { get; private set; }
        public Club Club { get; private set; }

        public static Result<JoinClub> Create(Guid userId, Guid clubId)
        {
            if(userId == Guid.Empty) return Result.Failure<JoinClub>(Error.NullValue);
            if(clubId == Guid.Empty) return Result.Failure<JoinClub>(Error.NullValue);

            var joinClub = new JoinClub(Guid.NewGuid(), userId, clubId);
            joinClub.CreatorId = userId;

            joinClub.RaiseDomainEvent(new JoinClubRequestSent(userId, clubId));

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
