using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Entities

{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType userType { get; set; } = UserType.Player;
        public decimal? ContractSalary;
        public Guid? TeamId;
    }
}
