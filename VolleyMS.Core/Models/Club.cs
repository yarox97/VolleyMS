using VolleyMS.Core.Common;
using VolleyMS.Core.DomainEvents;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class Club : BaseEntity
    {
        private readonly List<Task> _tasks = new();
        private readonly List<UserClub> _userClubs = new();
        private readonly List<JoinClub> _joinClubRequests = new();
        private Club() : base(Guid.Empty)
        {
        }
        private Club(Guid id,
            string name,
            string joinCode,
            string? description,
            string? avatarUrl,
            string? backGroundUrl)
            : base(id)
        {
            Name = name;
            Description = description;
            JoinCode = joinCode;
            AvatarUrl = avatarUrl;
            BackGroundURL = backGroundUrl;
        }
        public string Name { get; private set; }
        public string JoinCode { get; private set; }
        public string? Description { get; private set; }
        public string? AvatarUrl { get; private set; }
        public string? BackGroundURL { get; private set; }
        public IReadOnlyCollection<Task> Tasks => _tasks;
        public IReadOnlyCollection<UserClub> UserClubs => _userClubs;
        public IReadOnlyCollection<JoinClub> JoinClubRequests => _joinClubRequests;

        public static Result<Club> Create(string name, string joinCode, string? description, string? avatarUrl, string? backGroundUrl, User creator)
        {
            if(string.IsNullOrEmpty(name)) return Result.Failure<Club>(DomainErrors.Club.NameEmpty);
            
            var club = new Club(Guid.NewGuid(), name, joinCode, description, avatarUrl, backGroundUrl);
            club.CreatorId = creator.Id;

            var adminResult = club.AddMember(creator, ClubMemberRole.Creator);
            if(adminResult.IsFailure) return Result.Failure<Club>(adminResult.Error);

            return Result.Success(club);
        }

        public Result<JoinClub> RequestToJoinClub(User user)
        {
            if (user is null) return Result.Failure<JoinClub>(Error.NullValue);

            if (_joinClubRequests.Any(jc => jc.UserId == user.Id 
                                         && jc.ClubId == Id 
                                         && jc.JoinClubRequestStatus == JoinClubRequestStatus.Pending)) 
                return Result.Failure<JoinClub>(DomainErrors.Club.JoinRequestAlreadyExists);

            var joinClubResult = JoinClub.Create(user.Id, this.Id);

            if (joinClubResult.IsFailure) return joinClubResult;

            _joinClubRequests.Add(joinClubResult.Value);
            UpdatedAt = DateTime.UtcNow;

            return joinClubResult;
        }

        public Result ApproveJoinClubRequest(User user, ClubMemberRole clubMemberRole)
        {
            if (user is null) return Result.Failure<JoinClub>(Error.NullValue);

            var joinClubRequest = _joinClubRequests.FirstOrDefault(jc =>
                jc.UserId == user.Id &&
                jc.JoinClubRequestStatus == JoinClubRequestStatus.Pending);

            if (joinClubRequest is null) 
                return Result.Failure<JoinClub>(DomainErrors.Club.JoinRequestNotFound);

            var approveResult = joinClubRequest.Approve();
            if(approveResult.IsFailure) 
                return Result.Failure<JoinClub>(approveResult.Error);

            var addMemberResult = AddMember(user, clubMemberRole);
            if(addMemberResult.IsFailure) 
                return Result.Failure<JoinClub>(addMemberResult.Error);

            RaiseDomainEvent(new JoinClubApprovedDomainEvent(user.Id, this.Id));

            return Result.Success();
        }

        public Result RejectJoinClubRequest(User user)
        {
            if (user is null) return Result.Failure<JoinClub>(Error.NullValue);

            var joinClubRequest = _joinClubRequests.FirstOrDefault(jc =>
                jc.UserId == user.Id &&
                jc.JoinClubRequestStatus == JoinClubRequestStatus.Pending);
            if (joinClubRequest is null) return Result.Failure<JoinClub>(DomainErrors.Club.JoinRequestNotFound);

            var approveResult = joinClubRequest.Reject();
            if (approveResult.IsFailure) return Result.Failure<JoinClub>(approveResult.Error);

            RaiseDomainEvent(new JoinClubRejectedDomainEvent(user.Id, this.Id));

            return Result.Success();
        }

        public Result<UserClub> AddMember(User user, ClubMemberRole clubMemberRole)
        {
            if (user is null) return Result.Failure<UserClub>(Error.NullValue);

            if (_userClubs.Any(uc => uc.UserId == user.Id && uc.ClubId == Id)) return Result.Failure<UserClub>(DomainErrors.Club.MemberAlreadyExists);

            var userClubResult = UserClub.Create(user, this);
            if (userClubResult.IsFailure) return userClubResult;

            _userClubs.Add(userClubResult.Value);
            UpdatedAt = DateTime.UtcNow;
            return userClubResult;
        }

        public Result<UserClub> ChangeMemberRole(User user, ClubMemberRole clubMemberRole)
        {
            if (user is null) return Result.Failure<UserClub>(Error.NullValue);

            var userClub = _userClubs.FirstOrDefault(uc => uc.UserId == user.Id);
            if (userClub == null) return Result.Failure<UserClub>(DomainErrors.Club.MemberNotFound);

            var changeRoleResult = userClub.ChangeRole(clubMemberRole);
            if (changeRoleResult.IsFailure) return Result.Failure<UserClub>(changeRoleResult.Error);

            UpdatedAt = DateTime.UtcNow;
            return Result.Success(userClub);
        }

        public Result RemoveMember(User user)
        {
            var userClub = _userClubs.FirstOrDefault(uc => uc.UserId == user.Id);
            if (userClub is null) return Result.Failure(DomainErrors.Club.MemberNotFound);

            _userClubs.Remove(userClub);
            return Result.Success();
        }
    }
}