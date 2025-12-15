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

        public static Result<UserClub> Create(User user, Club club, User creator) 
        {
            if (user == null || club == null || creator == null) 
                return Result.Failure<UserClub>(Error.NullValue);

            var userClub = new UserClub(Guid.NewGuid(), user, club);
            userClub.CreatorId = creator.Id;

            return Result.Success(userClub);
        }

        public Result ChangeRole(ClubMemberRole clubMemberRole)
        {
            if (!Enum.IsDefined(typeof(ClubMemberRole), clubMemberRole)) return Result.Failure<UserClub>(DomainErrors.Role.InvalidRole);

            ClubMemberRole = clubMemberRole;
            UpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }
    }
}
