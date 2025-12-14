using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class UserClub : BaseEntity
    {
        private UserClub() : base(Guid.Empty)
        {
        }
        private UserClub(Guid id, User user, Club club)
            : base(id)
        {
            User = user;
            UserId = user.Id;
            Club = club;
            ClubId = club.Id;
            ClubMemberRole = ClubMemberRole.Player;
        }
        public User User { get; private set; }
        public Guid UserId { get; private set; }
        public Club Club { get; private set; }
        public Guid ClubId { get; private set; }
        public ClubMemberRole ClubMemberRole { get; private set; }

        public static Result<UserClub> Create(User user, Club club) 
        {
            if (user == null || club == null) return Result.Failure<UserClub>(Error.NullValue);

            return new UserClub(Guid.NewGuid(), user, club);
        }

        public Result ChangeRole(ClubMemberRole clubMemberRole)
        {
            if (!Enum.IsDefined(typeof(ClubMemberRole), clubMemberRole)) return Result.Failure<UserClub>(DomainErrors.Role.InvalidRole);

            ClubMemberRole = clubMemberRole;

            return Result.Success();
        }
    }
}
