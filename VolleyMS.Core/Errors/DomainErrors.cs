using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Errors
{
    public static class DomainErrors
    {
        public static class User
        {
            public static readonly Error UserNameEmpty = new(
                "User.UserNameEmpty",
                "Username cannot be null or empty.");

            public static readonly Error PasswordEmpty = new(
                "User.PasswordEmpty",
                "Password cannot be null or empty.");

            public static readonly Error NameEmpty = new(
                "User.NameEmpty",
                "Name cannot be null or empty.");

            public static readonly Error SurnameEmpty = new(
                "User.SurnameEmpty",
                "Surname cannot be null or empty.");

            public static readonly Error InvalidUserType = new(
                "User.InvalidUserType",
                "Invalid user type.");

            public static readonly Error SamePassword = new(
                "User.SamePassword",
                "Password cannot be the same as the previous one.");

            internal static Error EmailEmpty = new(
                "User.EmailEmpty",
                "Email cannot be null or empty.");

            public static Error UserNameTaken = new(
                "User.UserNameTaken",
                "User name is already taken.");

            public static Error UserNotFound = new(
                "User.UserNotFound",
                "User not found.");

            public static Error InvalidPassword = new(
                "User.InvalidPassword",
                "Invalid password.");
        }
        public static class Club
        {
            public static readonly Error NameEmpty = new(
                "Club.NameEmpty",
                "Club name cannot be null or empty.");

            public static readonly Error JoinCodeEmpty = new(
                "Club.JoinCodeEmpty",
                "Club join code cannot be null or empty.");

            public static readonly Error NameTooLong = new(
                "Club.NameTooLong",
                "Club name is too long.");

            public static readonly Error DescriptionTooLong = new(
                "Club.DescriptionTooLong",
                "Club description is too long.");

            public static readonly Error MemberAlreadyExists = new(
                "Club.MemberAlreadyExists",
                "The user is already a member of the club.");
            public static readonly Error InvalidClubMemberRole;
            public static Error MemberNotFound = new(
                "Club.MemberNotFound",
                "The user is not a member of the club.");

            public static Error InvalidPermission = new(
                "Club.InvalidPermission",
                "User does not have permission to perform this action.");
            public static Error JoinRequestAlreadyExists;

            public static Error InvalidMemberRole { get; internal set; }

            public static Error JoinRequestNotFound { get; internal set; }
            public static Error InvalidJoinCode { get; internal set; }
        }

        public static class JoinClubRequest
        {
            public static readonly Error InvalidStatus = new(
                "JoinClub.InvalidStatus",
                "Invalid join club request status.");
            public static readonly Error NotPending = new(
                "JoinClub.NotPending",
                "Cannot access not pending request.");
            public static Error ClubNotFound;
        }

        public static class Role
        {
            public static readonly Error InvalidRole = new(
                "Role.InvalidRole",
                "Invalid club member role.");
        }

        public static class Contract
        {
            public static readonly Error NegativeSalary = new(
                "Contract.NegativeSalary",
                "Monthly salary cannot be negative.");

            public static readonly Error InvalidDates = new(
                "Contract.InvalidDates",
                "Contract start date cannot be later than end date.");
        }

        public static class Task
        {
            public static readonly Error TitleEmpty = new(
                "Task.TitleEmpty",
                "Task title cannot be empty.");

            public static readonly Error InvalidDates = new(
                "Task.InvalidDates",
                "End date cannot be earlier than start date or current time.");

            public static readonly Error InvalidTimes = new(
                "Task.InvalidTimes",
                "Start time cannot be later than end time.");
        }

        public static class Comment
        {
            public static readonly Error TextEmpty = new(
                "Comment.TextEmpty",
                "Comment text cannot be empty.");
            public static readonly Error NotAuthor = new(
                "Comment.NotAuthor",
                "Only the author can modify or delete the comment.");
        }

        public static class Notification
        {
            public static readonly Error TextEmpty = new(
                "Notification.TextEmpty",
                "Notification text cannot be empty.");

            public static readonly Error RolesNull = new(
                "Notification.RolesNull",
                "Required club member roles cannot be null.");

            public static Error RolesEmpty { get; internal set; }
        }
    }
}
