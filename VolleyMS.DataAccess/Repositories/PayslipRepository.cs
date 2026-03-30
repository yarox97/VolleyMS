using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models.Payslips;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.DataAccess.Repositories
{
    public class PayslipRepository : GenericRepository<Payslip>, IPayslipRepository
    {
        public PayslipRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }

        public async Task<PaginatedResult<Payslip>> GetByUserIdWithDetailsAsync(
        Guid userId,
        int page,
        int pageSize,
        Guid? contractId,
        CancellationToken cancellationToken)
        {
            if (contractId == null)
            {
                var now = DateTime.UtcNow;

                var query = _volleyMsDbContext.Payslips
                    .AsNoTracking()
                    .Include(p => p.Adjustments)
                    .Include(p => p.Contract)
                        .ThenInclude(c => c.Club)
                    .Where(p => p.UserId == userId && p.Year == now.Year && p.Month == now.Month)
                    .OrderByDescending(p => p.Year)
                    .ThenByDescending(p => p.Month);

                var totalCount = await query.CountAsync(cancellationToken);

                var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                return new PaginatedResult<Payslip>(items, totalCount, page, pageSize);
            }
            else
            {
                var query = _volleyMsDbContext.Payslips
                    .AsNoTracking()
                    .Include(p => p.Adjustments)
                    .Include(p => p.Contract)
                        .ThenInclude(c => c.Club)
                    .Where(p => p.UserId == userId && p.Contract.Id == contractId)
                    .OrderByDescending(p => p.Year)
                    .ThenByDescending(p => p.Month);

                var totalCount = await query.CountAsync(cancellationToken);

                var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                return new PaginatedResult<Payslip>(items, totalCount, page, pageSize);
            }
        }

        public async Task<PaginatedResult<Payslip>> GetByClubIdWithDetailsAsync(
        Guid clubId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
        {
            var query = _volleyMsDbContext.Payslips
                .AsNoTracking()
                .Include(p => p.Adjustments)
                .Include(p => p.Contract)
                    .ThenInclude(c => c.Club)
                .Include(p => p.User)
                .Where(p => p.Contract.ClubId == clubId)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<Payslip>(items, totalCount, page, pageSize);
        }

        public async Task<Payslip?> GetPayslipByIdWithDetailsAsync(Guid payslipId, CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.Payslips
                .Include(p => p.Adjustments)
                .Include(p => p.Contract)
                    .ThenInclude(c => c.Club)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == payslipId, cancellationToken);
        }

        public Task<Payslip?> GetByUserIdAndMonthYearAsync(Guid userId, int month, int year, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedResult<PayslipAdjustment>> GetByPayslipIdWithDetailsAsync(Guid payslipId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _volleyMsDbContext.PayslipAdjustments
                .AsNoTracking()
                .Include(pa => pa.Payslip)
                    .ThenInclude(pa => pa.Contract)
                        .ThenInclude(c => c.User)
                .Include(pa => pa.Payslip)
                    .ThenInclude(p => p.Contract)
                        .ThenInclude(c => c.Club)
                .Where(p => p.Payslip.Id == payslipId)
                .OrderByDescending(p => p.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<PayslipAdjustment>(items, totalCount, page, pageSize);
        }

        public async Task<PaginatedResult<Payslip>> GetByContractIdWithDetailsAsync(Guid contractId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _volleyMsDbContext.Payslips
                .Include(p => p.Contract)
                    .ThenInclude(c => c.Club)
                .Include(p => p.Adjustments)
                .Where(p => p.ContractId == contractId) 
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<Payslip>(items, totalCount, page, pageSize);
        }

        public async Task<List<PayslipAdjustment>> GetAdjustmentsByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.PayslipAdjustments
                .Include(a => a.Payslip)
                    .ThenInclude(p => p.Contract)
                .Where(a => ids.Contains(a.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
