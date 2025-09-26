using System.Runtime.CompilerServices;
using VolleyMS.Core.Common;

namespace VolleyMS.Core.Models
{
    public class User : AuditableFields
    {
        private User(Guid id, string userName, string password, UserType userType, string name, string surname)
        {
            Id = id;
            UserName = userName;
            Password = password;
            UserType = userType;
            Name = name;
            Surname = surname;
        }

        public Guid Id { get; }
        public string UserName { get; } = string.Empty;
        public string Password { get; } = string.Empty;
        public UserType UserType { get; } = UserType.Player;
        public string Name { get; } = string.Empty;
        public string Surname { get; } = string.Empty;

        public static (User user, string error) Create(Guid id, string userName, string password, 
                                                      UserType userType, string name, string surname)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(userName))
            {
                error = "User name is required";
            }
            if (string.IsNullOrEmpty(password))
            {
                error = "Password is required";
            }
            if (userType != UserType.Player && userType != UserType.Admin && userType != UserType.Coach)
            {
                error = "Invalid user type";
            }
            if(string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(name))
            {
                error = "Invalid name or surname";
            }
            
            var user = new User(id, userName, password, userType, name, surname);
            return (user, error);
        }
    }
}
