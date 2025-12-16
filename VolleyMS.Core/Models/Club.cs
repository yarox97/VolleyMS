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
        private readonly List<JoinClubRequest> _joinClubRequests = new();
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
        public IReadOnlyCollection<JoinClubRequest> JoinClubRequests => _joinClubRequests;

        public static Result<Club> Create(string name, string joinCode, string? description, string? avatarUrl, string? backGroundUrl, User creator)
        {
            if(string.IsNullOrEmpty(name)) return Result.Failure<Club>(DomainErrors.Club.NameEmpty);
            
            var club = new Club(Guid.NewGuid(), name, joinCode, description, avatarUrl, backGroundUrl);
            club.CreatorId = creator.Id;

            var adminResult = club.AddMember(creator, ClubMemberRole.Creator, creator);
            if(adminResult.IsFailure) return Result.Failure<Club>(adminResult.Error);

            return Result.Success(club);
        }

        public Result<JoinClubRequest> RequestToJoinClub(User user)
        {
            if (user is null) return Result.Failure<JoinClubRequest>(Error.NullValue);

            if (_joinClubRequests.Any(jc => jc.UserId == user.Id 
                                         && jc.ClubId == Id 
                                         && jc.JoinClubRequestStatus == JoinClubRequestStatus.Pending)) 
                return Result.Failure<JoinClubRequest>(DomainErrors.Club.JoinRequestAlreadyExists);

            if (_userClubs.Any(uc => uc.UserId == user.Id && uc.ClubId == Id))
                return Result.Failure<JoinClubRequest>(DomainErrors.Club.MemberAlreadyExists);

            var joinClubResult = JoinClubRequest.Create(user.Id, this.Id);

            if (joinClubResult.IsFailure) return joinClubResult;

            _joinClubRequests.Add(joinClubResult.Value);

            RaiseDomainEvent(new JoinClubRequestSentDomainEvent(user.Id, this.Id, user.Name, user.Surname, this.Name)); // Send notification to users who can approve join requests

            return joinClubResult;
        }

        private bool IsClubAdminOrPresident(Guid userId)
        {
            return _userClubs.Any(u => u.UserId == userId &&
                   (u.ClubMemberRole == ClubMemberRole.President || u.ClubMemberRole == ClubMemberRole.Creator));
        }

        public Result ApproveJoinRequest(JoinClubRequest request, ClubMemberRole roleToAdd, User responser)
        {
            if (!IsClubAdminOrPresident(responser.Id))
                return Result.Failure(DomainErrors.Club.InvalidPermission);

            if (request.ClubId != this.Id)
                return Result.Failure(DomainErrors.Club.JoinRequestNotFound);

            var requestResult = request.Approve(responser);

            if (requestResult.IsFailure)
                return requestResult;

            var newUserMembership = AddMember(request.User, roleToAdd, responser);

            if(newUserMembership.IsFailure)
                return newUserMembership;

            RaiseDomainEvent(new JoinClubApprovedDomainEvent(request.UserId, request.ClubId, responser.Id, this.Name)); // Send approval notification to requestor

            return Result.Success();
        }

        public Result RejectJoinClubRequest(JoinClubRequest request, User responser)
        {
            if (request.User is null) 
                return Result.Failure(DomainErrors.User.UserNotFound);

            if (request is null) 
                return Result.Failure<JoinClubRequest>(DomainErrors.Club.JoinRequestNotFound);

            if (request.JoinClubRequestStatus != JoinClubRequestStatus.Pending) 
                return Result.Failure<JoinClubRequest>(DomainErrors.JoinClubRequest.NotPending);

            if (!IsClubAdminOrPresident(responser.Id)) 
                return Result.Failure<JoinClubRequest>(DomainErrors.Club.InvalidPermission);

            var requestResult = request.Reject(responser);

            if (requestResult.IsFailure) 
                return Result.Failure(requestResult.Error);

            RaiseDomainEvent(new JoinClubRejectedDomainEvent(request.UserId, this.Id, responser.Id, this.Name, responser.Id)); // Send rejection notification to requestor 

            return Result.Success();
        }

        public Result<UserClub> AddMember(User user, ClubMemberRole clubMemberRole, User creator)
        {
            if (user is null) return Result.Failure<UserClub>(DomainErrors.User.UserNotFound);

            if (_userClubs.Any(uc => uc.UserId == user.Id && uc.ClubId == Id)) return Result.Failure<UserClub>(DomainErrors.Club.MemberAlreadyExists);

            var userClubResult = UserClub.Create(user, this, creator, clubMemberRole);
            if (userClubResult.IsFailure) return userClubResult;

            _userClubs.Add(userClubResult.Value);

            return userClubResult;

        }

        public Result<UserClub> ChangeMemberRole(User user, ClubMemberRole clubMemberRole)
        {
            if (user is null) return Result.Failure<UserClub>(Error.NullValue);

            var userClub = _userClubs.FirstOrDefault(uc => uc.UserId == user.Id);
            if (userClub == null) return Result.Failure<UserClub>(DomainErrors.Club.MemberNotFound);

            var changeRoleResult = userClub.ChangeRole(clubMemberRole);
            if (changeRoleResult.IsFailure) return Result.Failure<UserClub>(changeRoleResult.Error);

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