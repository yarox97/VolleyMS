using VolleyMS.Core.Common;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Entities

{
    public class UserEntity : AuditableFields
    {
        public UserEntity()
        {
            ContractEntities = new List<ContractEntity>();
            ClubEntities = new List<ClubEntity>();
        }
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType userType { get; set; } = UserType.Player;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public IList<ContractEntity> ContractEntities { get; set; }
        public IList<ClubEntity> ClubEntities { get; set; }
    }
}
