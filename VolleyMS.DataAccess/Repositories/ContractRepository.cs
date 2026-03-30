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

        public async Task<(IEnumerable<Contract> Items, int TotalCount)> GetUserContractsPaged(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var baseQuery = _volleyMsDbContext.Contracts
                .Where(c => c.UserId == userId);

            var totalCount = await baseQuery.CountAsync(cancellationToken);

            var items = await baseQuery
                .AsNoTracking()
                .Include(c => c.Club)
                .Include(c => c.Assigner)
                .OrderBy(c => c.EndsBy < now)
                .ThenByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(IEnumerable<Contract> Items, int TotalCount)> GetClubContractsPaged(Guid clubId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var baseQuery = _volleyMsDbContext.Contracts
                .Where(c => c.ClubId == clubId);

            var totalCount = await baseQuery.CountAsync(cancellationToken);

            var items = await baseQuery
                .AsNoTracking()
                .Include(c => c.Club)
                .Include(c => c.User)   
                .Include(c => c.Assigner)
                .OrderBy(c => c.EndsBy < now)
                .ThenByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Contract> GetByIdWithDetailsAsync(Guid contractId, CancellationToken cancellationToken)
        {
            var contract = await _volleyMsDbContext.Contracts
                .AsNoTracking()
                .Include(c => c.Club)
                .Include(c => c.User)
                .Include(c => c.Assigner)
                .FirstOrDefaultAsync(c => c.Id == contractId, cancellationToken);

            if (contract == null)
            {
                throw new InvalidOperationException($"Contract with id {contractId} not found.");
            }

            return contract;
        }

        public async Task<Contract?> GetByUserIdAndClubId(Guid userId, Guid clubId, CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.Contracts
                .AsNoTracking()
                .Include(c => c.Club)
                .Include(c => c.User)
                .Where(c => c.UserId == userId && c.ClubId == clubId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
