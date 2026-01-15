using VolleyMS.Core.Models;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Repositories
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        public Task<Result<Contract>> GetActiveContractByUserAndClubAsync(Guid userId, Guid clubId, CancellationToken cancellationToken);
    }
}
