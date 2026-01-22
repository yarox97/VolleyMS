using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.DataAccess.Repositories
{
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }

        public async Task<Contract?> GetActiveContractByUserAndClubAsync(Guid userId, Guid clubId, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            return await _volleyMsDbContext.Contracts
                .AsNoTracking()
                .Include(c => c.Club)
                .Include(c => c.User)
                .Where(c => c.UserId == userId && c.ClubId == clubId && now > c.EndsBy)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
