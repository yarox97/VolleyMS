using System.Runtime.CompilerServices;

namespace VolleyMS.Core.Models
{
    public enum UserType
    {
        Admin,
        Coach,
        Player
    }
    public class User
    {
        private User(Guid id, string userName, string password, UserType userType, decimal? contractSalary, Guid? teamId)
        {
            Id = id;
            UserName = userName;
            Password = password;
            UserType = userType;
            ContractSalary = contractSalary;
            TeamId = teamId;
        }

        public Guid Id { get; }
        public string UserName { get; } = string.Empty;
        public string Password { get; } = string.Empty;
        public UserType UserType { get; } = UserType.Player;
        public decimal? ContractSalary;
        public Guid? TeamId; 

        public static (User user, string error) Create(Guid id, string userName, string password, 
                                                      UserType userType, decimal? contractSalary, Guid? teamId)
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
            if(contractSalary is not null and < 0)
            {
                error = "Contract salary cannot be negative";
            }
            var user = new User(id, userName, password, userType, contractSalary, teamId);
            return (user, error);
        }
    }
}
