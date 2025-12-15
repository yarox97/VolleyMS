using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class JoinClubRequest : BaseEntity
    {
        private JoinClubRequest() : base(Guid.Empty)
        {
        }
        private JoinClubRequest(Guid id, Guid userId, Guid clubId)
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
        public Guid? ResponserId { get; private set; }
        public User? Responser { get; private set; }

        public static Result<JoinClubRequest> Create(Guid userId, Guid clubId)
        {
            if(userId == Guid.Empty) return Result.Failure<JoinClubRequest>(Error.NullValue);
            if(clubId == Guid.Empty) return Result.Failure<JoinClubRequest>(Error.NullValue);

            var joinClub = new JoinClubRequest(Guid.NewGuid(), userId, clubId);
            joinClub.CreatorId = userId;

            return Result.Success(joinClub);
        }

        public Result Approve(User responser)
        {
            if (JoinClubRequestStatus != JoinClubRequestStatus.Pending) 
                return Result.Failure(DomainErrors.JoinClubRequest.NotPending);

            JoinClubRequestStatus = JoinClubRequestStatus.Approved;
            ResponserId = responser.Id;
            Responser = responser;

            return Result.Success();
        }
        public Result Reject(User responser)
        {
            if (JoinClubRequestStatus != JoinClubRequestStatus.Pending) 
                return Result.Failure(DomainErrors.JoinClubRequest.NotPending);

            JoinClubRequestStatus = JoinClubRequestStatus.Rejected;
            ResponserId = responser.Id;
            Responser = responser;

            return Result.Success();
        }
    }
}
