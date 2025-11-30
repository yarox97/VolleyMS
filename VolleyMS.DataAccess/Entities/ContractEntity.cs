using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities
{
    public class ContractEntity : BaseEntity
    {
        public ContractEntity() : base(Guid.NewGuid())
        {
        }
        public decimal? MontlySalary { get; set; }
        public Currency? Currency { get; set; }
        public DateTime BeginsFrom { get; set; }
        public DateTime EndsBy { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public Guid TeamId { get; set; }
        public ClubEntity Club { get; set; }
    }
}