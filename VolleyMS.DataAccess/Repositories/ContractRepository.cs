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

        public async Task<Result<Contract>> GetActiveContractByUserAndClubAsync(Guid userId, Guid clubId, CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.Contracts
                .Include(c => c.User)
                .Include(c => c.Club)
                .FirstOrDefaultAsync(c => c.User.Id == userId && c.Club.Id == clubId && c.IsActive, cancellationToken)
                is Contract contract
                    ? Result.Success(contract)
                    : Result.Failure<Contract>(DomainErrors.Contract.ActiveContractNotFound);
        }
    }
}
