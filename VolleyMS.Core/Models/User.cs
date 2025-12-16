using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;
using static VolleyMS.Core.Errors.DomainErrors;

namespace VolleyMS.Core.Models
{
    public class User : BaseEntity
    {
        private readonly List<UserClub> _userClubs = new();
        private readonly List<Contract> _contracts = new();
        private readonly List<Task> _sentTasks = new();
        private readonly List<Task> _receivedTasks = new();
        private readonly List<Comment> _sentComments = new();
        private readonly List<Notification> _sentNotifications = new();
        private readonly List<UserNotification> _receivedNotifications = new();
        private readonly List<JoinClubRequest> _joinClubRequests = new();
        private User() : base(Guid.Empty) 
        {
        }
        private User(Guid id, 
            string userName, 
            string password, 
            UserType userType, 
            string name, 
            string surname,
            string email,
            string? avatarUrl)
            : base(id)
        {
            UserName = userName;
            Password = password;
            UserType = userType;
            Name = name;
            Surname = surname;
            Email = email;
            AvatarUrl = avatarUrl;
        }
        public string UserName { get; private init; }
        public string Password { get; private set; }
        public UserType UserType { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string? AvatarUrl { get; private set; }
        public IReadOnlyCollection<UserClub> UserClubs => _userClubs;
        public IReadOnlyCollection<Contract> Contracts => _contracts;
        public IReadOnlyCollection<Task> SentTasks => _sentTasks;
        public IReadOnlyCollection<Task> ReceivedTasks => _receivedTasks;
        public IReadOnlyCollection<Comment> SentComments => _sentComments;
        public IReadOnlyCollection<Notification> SentNotifications => _sentNotifications;
        public IReadOnlyCollection<UserNotification> ReceivedNotifications => _receivedNotifications;
        public IReadOnlyCollection<JoinClubRequest> JoinClubRequests => _joinClubRequests;

        public Result<User> ChangePassword(string newHashedPassword)
        {
            if (string.IsNullOrWhiteSpace(newHashedPassword)) return Result.Failure<User>(DomainErrors.User.PasswordEmpty);

            if (Password == newHashedPassword) return Result.Failure<User>(DomainErrors.User.SamePassword);

            Password = newHashedPassword;
            return Result.Success(this);
        }

        public static Result<User> Create(
            Guid id,
            string userName, 
            string password, 
            UserType userType, 
            string name, 
            string surname,
            string email,
            string? avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(userName)) 
                return Result.Failure<User>(DomainErrors.User.UserNameEmpty);

            if (string.IsNullOrWhiteSpace(password)) 
                return Result.Failure<User>(DomainErrors.User.PasswordEmpty);

            if (!Enum.IsDefined(typeof(UserType), userType)) 
                return Result.Failure<User>(DomainErrors.User.InvalidUserType);

            if (string.IsNullOrWhiteSpace(surname)) 
                return Result.Failure<User>(DomainErrors.User.SurnameEmpty);

            if (string.IsNullOrWhiteSpace(name)) 
                return Result.Failure<User>(DomainErrors.User.NameEmpty);

            if (string.IsNullOrEmpty(email))
                return Result.Failure<User>(DomainErrors.User.EmailEmpty);

            var user = new User(Guid.NewGuid(), userName, password, userType, name, surname, email, avatarUrl);

            user.RaiseDomainEvent(new DomainEvents.UserCreatedDomainEvent(user.Id));

            return Result.Success(user);
        }
        public Result UpdateDetails(string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(DomainErrors.User.NameEmpty);

            if (string.IsNullOrWhiteSpace(surname))
                return Result.Failure(DomainErrors.User.SurnameEmpty);

            Name = name;
            Surname = surname;

            return Result.Success();
        }

        public Result<ClubMemberRole> GetRoleInClub(Guid clubId)
        {
            var userClub = _userClubs.FirstOrDefault(uc => uc.ClubId == clubId && uc.UserId == this.Id);
            if (userClub == null)
                return Result.Failure<ClubMemberRole>(DomainErrors.Club.MemberNotFound);

            return Result.Success(userClub.ClubMemberRole);
        }

        public Result ChangeRole(UserType newRole)
        {
            if (!Enum.IsDefined(typeof(UserType), newRole)) 
                return Result.Failure(DomainErrors.User.InvalidUserType);

            UserType = newRole;
            return Result.Success();
        }
    }
}
