using System.Runtime.CompilerServices;
using VolleyMS.Core.Common;
using VolleyMS.Core.Exceptions;

namespace VolleyMS.Core.Models
{
    public class User : BaseEntity
    {
        private User(Guid id, 
            string userName, 
            string password, 
            UserType userType, 
            string name, 
            string surname)
            : base(id)
        {
            UserName = userName;
            Password = password;
            UserType = userType;
            Name = name;
            Surname = surname;
        }
        public string UserName { get; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public UserType UserType { get; } = UserType.Player;
        public string Name { get; } = string.Empty;
        public string Surname { get; } = string.Empty;

        public void SetHashedPassword(string hashedPassword)
        {
            Password = hashedPassword;
        }

        public static User Create(Guid id, 
            string userName, 
            string password, 
            UserType userType, 
            string name, 
            string surname)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new EmptyFieldDomainException("Username cannot be null or empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new EmptyFieldDomainException("Password cannot be null or empty");
            }
            if (userType != UserType.Player && userType != UserType.Admin)
            {
                throw new InvalidUserTypeDomainException("Invalid user type");
            }
            if(string.IsNullOrEmpty(surname))
            {
                throw new EmptyFieldDomainException("Surname cannot be null or empty");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new EmptyFieldDomainException("Name cannot be null or empty");
            }
            
            var user = new User(id, userName, password, userType, name, surname);
            return user;
        }
    }
}
