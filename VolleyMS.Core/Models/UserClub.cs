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
        private UserClub(Guid id, User user, Club club, ClubMemberRole clubMemberRole)
            : base(id)
        {
            User = user;
            UserId = user.Id;
            Club = club;
            ClubMemberRole = clubMemberRole;
            ClubId = club.Id;
            ClubMemberRole = ClubMemberRole.Player;
        }
        public User User { get; private set; }
        public Guid UserId { get; private set; }
        public Club Club { get; private set; }
        public Guid ClubId { get; private set; }
        public ClubMemberRole ClubMemberRole { get; private set; }

        public static Result<UserClub> Create(User user, Club club, User creator, ClubMemberRole clubMemberRole) 
        {
            if (user == null || club == null || creator == null) 
                return Result.Failure<UserClub>(Error.NullValue);

            if (!Enum.IsDefined(typeof(ClubMemberRole), clubMemberRole)) 
                return Result.Failure<UserClub>(DomainErrors.Role.InvalidRole);

            var userClub = new UserClub(Guid.NewGuid(), user, club, clubMemberRole);
            userClub.CreatorId = creator.Id;

            return Result.Success(userClub);
        }

        public Result ChangeRole(ClubMemberRole clubMemberRole)
        {
            if (!Enum.IsDefined(typeof(ClubMemberRole), clubMemberRole)) return Result.Failure<UserClub>(DomainErrors.Role.InvalidRole);

            ClubMemberRole = clubMemberRole;

            return Result.Success();
        }
    }
}
