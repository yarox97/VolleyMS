using VolleyMS.Core.Common;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Entities
{
    public class ContractEntity : AuditableFields
    {
        public ContractEntity()
        {
            UserEntity = new UserEntity();
            ClubEntity = new ClubEntity();
        }
        public Guid Id { get; set; }
        public decimal? MontlySalary { get; set; }
        public Currency? Currency { get; set; }
        public DateTime BeginsFrom { get; set; }
        public DateTime EndsBy { get; set; }

        public Guid UserId { get; set; }
        public UserEntity UserEntity { get; set; }
        public Guid TeamId { get; set; }
        public ClubEntity ClubEntity { get; set; }
    }
}