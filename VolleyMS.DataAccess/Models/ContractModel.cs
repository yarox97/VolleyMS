using VolleyMS.Core.Common;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Entities
{
    public class ContractModel : BaseEntity
    {
        public ContractModel()
        {
            UserModel = new UserModel();
            ClubModel = new ClubModel();
        }
        public Guid Id { get; set; }
        public decimal? MontlySalary { get; set; }
        public Currency? Currency { get; set; }
        public DateTime BeginsFrom { get; set; }
        public DateTime EndsBy { get; set; }

        public Guid UserId { get; set; }
        public UserModel UserModel { get; set; }
        public Guid TeamId { get; set; }
        public ClubModel ClubModel { get; set; }
    }
}