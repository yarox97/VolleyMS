using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Entities

{
    public class UserEntity
    {
        public UserEntity()
        {
            Contracts = new List<Contract>();
            Clubs = new List<Club>();
        }
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType userType { get; set; } = UserType.Player;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public IList<Contract> Contracts { get; set; }
        public IList<Club> Clubs { get; set; }
    }
}
